// Making sure all animations play half speed

for (var state : AnimationState in GetComponent.<Animation>()) {
	state.speed =0.2;
}