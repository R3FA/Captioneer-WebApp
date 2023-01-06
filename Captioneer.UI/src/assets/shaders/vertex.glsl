#version 300 es

layout(location = 0) in vec3 aPos;
layout(location = 1) in ivec2 aTex;

out vec2 tex;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProj;

void main()
{
    gl_Position = uProj * uView * uModel * vec4(aPos, 1.0);
    tex = vec2(aTex);
}