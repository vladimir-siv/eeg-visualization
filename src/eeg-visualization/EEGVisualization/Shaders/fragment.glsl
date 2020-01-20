#version 430 core

uniform vec4 ambient_light_color;
uniform float ambient_light_power;

uniform vec4 light_source_position;
uniform vec4 light_source_color;
uniform float light_source_power;

uniform vec4 eye_position;

in vec4 position;	// fragment position
in vec4 color;		// fragment color
in vec4 normal;		// fragment normal

out vec4 out_color;

void main(void)
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
