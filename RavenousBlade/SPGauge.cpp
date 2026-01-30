#include "SPGauge.h"

float SPGauge::sumGauge = 0.0f;
float SPGauge::gaugeProgress = 0.25f;
bool SPGauge::isGaugeMax = false;

void SPGauge::setScale(float value){
	gaugeSprite.setScale(value, value);
	gaugeMaskSprite.setScale(value, value);
}

bool SPGauge::loadAllTexture(){
	if(!gaugeTexture_def.loadFromFile("./image/gauge_def.png")) return false;
	if(!gaugeTexture_def_mask.loadFromFile("./image/gauge_def_mask.png")) return false;
	if(!gaugeTexture_max.loadFromFile("./image/gauge_max.png")) return false;
	return true;
}

void SPGauge::setTexture(){
	gaugeMaskSprite.setTexture(gaugeTexture_def_mask);
	if(sumGauge >= 1.0f){
		sumGauge = 1.0f;
		isGaugeMax = true;
		gaugeSprite.setTexture(gaugeTexture_max);
	}else{
		isGaugeMax = false;
		gaugeSprite.setTexture(gaugeTexture_def);
	}
}

void SPGauge::updateMaskSprite(){
	setTexture();
	sf::Vector2u texSize = gaugeTexture_def_mask.getSize();
	int maskHeight = texSize.y * (1.0f - sumGauge);
	gaugeMaskSprite.setTextureRect(sf::IntRect(
		0,
		0,
		texSize.x,
		maskHeight
	));
}

void SPGauge::reset(){
	sumGauge = 0.0f;
	gaugeProgress = 0.1f;
	isGaugeMax = false;
}