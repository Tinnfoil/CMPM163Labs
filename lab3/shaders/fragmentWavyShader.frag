varying vec3 vUv;

uniform vec3 colorA;
uniform vec3 colorB;
uniform vec3 colorC;
uniform vec3 colorD;


void main() {
	vec4 first = vec4(mix(colorA, colorB, vUv.x * gl_FragCoord.x), 1);
	vec4 second = vec4(mix(colorC, colorD, vUv.y * gl_FragCoord.y), 1);
	vec4 final = first * second;
	gl_FragColor = final;
}
