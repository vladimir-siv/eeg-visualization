#version 430 core

#define PI 3.141592653588979f

uniform vec4 ambient_light_color;
uniform float ambient_light_power;

uniform vec4 light_source_position;
uniform vec4 light_source_color;
uniform float light_source_power;

uniform vec4 eye_position;

uniform float electrode_positions[2048]; // grouped by 3
uniform uint electrode_count;
uniform uint electrode_indices[64];
uniform uint electrodes_used_count;
uniform float electrode_values[64];
uniform float electrode_max_distance;

in vec4 model_position;
in vec4 position;	// fragment position
in vec4 color;		// fragment color
in vec4 normal;		// fragment normal

out vec4 out_color;

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main(void)
{
	if (model_position.y >= 4.0f)
	{
		float hue = 0.7f;
		
		vec3 model_vector = normalize(vec3(model_position.xyz));
		for (int i = 0; i < electrodes_used_count; ++i)
		{
			uint index = electrode_indices[i] * 3;
			vec3 electrode_vector = vec3(electrode_positions[index + 0], electrode_positions[index + 1], electrode_positions[index + 2]);
			float angle = acos(dot(model_vector, electrode_vector)) * 180.0f / PI;
			
			// if (angle >= electrode_max_distance) gate = 0;
			// else gate = 1;
			int gate = int(1.0f - min(max(angle, electrode_max_distance) - electrode_max_distance, 1.0f));
			
			float value = (electrode_values[i] + 50.0f) / 100.0f;
			hue -= gate * value * electrode_max_distance / (5.0f * angle);
		}
		
		out_color = vec4(hsv2rgb(vec3(clamp(hue, 0.0f, 0.7f), 1.0f, 1.0f)), 1.0f);
	}
	else
	{
		vec4 light_vector = normalize(light_source_position - position);
		vec4 eye_vector = normalize(eye_position - position);
		float light_distance = distance(light_source_position, position);
		
		vec4 ambient = ambient_light_color * ambient_light_power;
		vec4 light_source = light_source_color * light_source_power;
		
		float diffuse_angle = clamp(dot(light_vector, normalize(normal)), 0.0f, 1.0f); // cos(angle)
		vec4 diffuse = vec4(diffuse_angle, diffuse_angle, diffuse_angle, 1.0f);
		
		float specular_angle = pow(clamp(dot(reflect(-light_vector, normal), eye_vector), 0.0f, 1.0f), 7.0f); // cos(angle)^7
		vec4 specular = vec4(specular_angle, specular_angle, specular_angle, 1.0f);
		
		out_color = color;
		out_color *= ambient + light_source * (diffuse + specular) / (1.05f + 0.05f * light_distance * light_distance);
		out_color.w = 1.0f;
		out_color = clamp(out_color, 0.0f, 1.0f);
	}
}
