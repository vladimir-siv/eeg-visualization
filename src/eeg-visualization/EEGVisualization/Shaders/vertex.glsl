#version 430 core

uniform mat4 project;
uniform mat4 view;
uniform mat4 translate;
uniform mat4 scale;
uniform mat4 rotate;

in layout(location = 0) vec4 in_position;
in layout(location = 1) vec4 in_color;
in layout(location = 2) vec4 in_normal;

out vec3 model_position;
out vec3 position;
out vec3 color;
out vec3 normal;

void main(void)
{
	vec4 world_position = translate * scale * rotate * in_position;
	vec4 world_normal = rotate * in_normal;
	
	model_position = in_position.xyz;
	position = world_position.xyz;
	color = in_color.rgb;
	normal = world_normal.xyz;
	
	gl_Position = project * view * world_position;
}
