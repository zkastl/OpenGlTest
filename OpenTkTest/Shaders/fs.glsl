#version 450

in VS_OUT
{
	vec4 color;
} fs_in;

out vec4 outputColor;

void main()
{
    outputColor = fs_in.color;
}