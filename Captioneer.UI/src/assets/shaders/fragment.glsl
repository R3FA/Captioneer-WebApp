#version 300 es

in highp vec2 tex;

out highp vec4 color;

uniform sampler2D uSampler;

void main()
{
    color = texture(uSampler, tex);
}