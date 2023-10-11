shader_type canvas_item;

uniform float circle_size : hint_range(0.0, 1.0) = 0.5;
uniform float screen_width;
uniform float screen_height;

void fragment() {
	float ratio = screen_width / screen_height;
	float dist = distance(vec2(0.5, 0.5), vec2(mix(0.5, UV.x, ratio), UV.y));
	COLOR.a = step(circle_size, dist);
	// Place fragment code here.
}
