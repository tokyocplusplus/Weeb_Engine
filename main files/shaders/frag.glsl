#version 330 core
out vec4 fg;
uniform vec4 uniColor;
in vec3 mainColor;
void main()
{
    fg = vec4(mainColor, 1); //vec4(uniColor);
}