#version 430 core

uniform mat4 project;
uniform mat4 view;
uniform mat4 translate;
uniform mat4 scale;
uniform mat4 rotate;

in layout(location = 0) vec4 in_position;
in layout(location = 1) vec4 in_color;
in layout(location = 2) vec4 in_normal;

out vec4 model_position;
out vec4 position;
out vec4 color;
out vec4 normal;

void main(void)
{
	model_position = in_position;
	position = translate * scale * rotate * in_position;
	color = in_color;
	normal = rotate * in_normal;
	
	gl_Position = project * view * position;
}
