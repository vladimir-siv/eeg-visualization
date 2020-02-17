#version 430 core

#define PI 3.141592653588979f

uniform vec3 ambient_light_color;
uniform float ambient_light_power;

uniform vec3 light_source_position;
uniform vec3 light_source_color;
uniform float light_source_power;

uniform vec3 eye_position;

uniform float electrode_positions[2048]; // grouped by 3
uniform uint electrode_count;
uniform uint electrode_indices[64];
uniform uint electrodes_used_count;
uniform float electrode_values[64];
uniform float electrode_max_distance;

in vec3 model_position;
in vec3 position;	// fragment position
in vec3 color;		// fragment color
in vec3 normal;		// fragment normal

out vec4 out_color;

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main(void)
{
	const float ylimit = 4.0f;
	out_color = vec4(0.0f, 0.0f, 0.0f, 0.0f);

	// EEG
	float hue = 0.75f;
	vec3 model_vector = normalize(model_position);
	for (int i = 0; i < electrodes_used_count; ++i)
	{
		uint index = electrode_indices[i] * 3;
		vec3 electrode_vector = vec3(electrode_positions[index + 0], electrode_positions[index + 1], electrode_positions[index + 2]);
		float angle = acos(dot(model_vector, electrode_vector)) * 180.0f / PI;

		float value = (electrode_values[i] + 50.0f) / 100.0f;
		hue -= int(angle < electrode_max_distance) * value * electrode_max_distance / (5.0f * angle);
	}
	
	out_color += int(model_position.y >= ylimit) * vec4(hsv2rgb(vec3(clamp(hue, 0.0f, 0.75f), 1.0f, 1.0f)), 1.0f);

	// Phong
	vec3 normal_vector = normalize(normal);
	vec3 light_vector = normalize(light_source_position - position);
	vec3 eye_vector = normalize(eye_position - position);
	float light_distance = distance(light_source_position, position);
	float attenuation = 1.05f + 0.05f * light_distance * light_distance;
	
	vec3 ambient = ambient_light_color * ambient_light_power;
	vec3 light_source = light_source_color * light_source_power;
	
	float diffuse = clamp(dot(light_vector, normal_vector), 0.0f, 1.0f); // cos(angle)
	float specular = pow(clamp(dot(reflect(-light_vector, normal_vector), eye_vector), 0.0f, 1.0f), 21.0f); // cos(angle)^21
	
	vec3 light = color * (ambient + light_source * (diffuse + specular) / attenuation);
	out_color += int(model_position.y < ylimit) * vec4(light, 1.0f);
}
