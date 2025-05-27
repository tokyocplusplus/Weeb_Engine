#version 330 core
out vec4 fg;
uniform vec4 uniColor;
in vec3 mainColor;
void main()
{
    float resX = 1280.0f;
    float resY = 720.0f;
    vec2 res = vec2(resX,resY);
    vec3 COLOR_pink = vec3(2,0,2);
    vec3 COLOR_blue = vec3(0,1,1);
    vec2 st = gl_FragCoord.xy / res.xy;
    float linear_modifier = st.y;
    float weight = linear_modifier;
    vec3 canvas = mix(COLOR_pink,COLOR_blue,weight);
    fg = vec4(canvas,1);
}
