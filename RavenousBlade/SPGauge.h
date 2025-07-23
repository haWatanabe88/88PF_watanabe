#pragma once
#include <SFML/Graphics.hpp>

class SPGauge{
	private:
		sf::Texture gaugeTexture_def;
		sf::Texture gaugeTexture_def_mask;
		sf::Texture gaugeTexture_max;
		sf::Sprite gaugeSprite;
		sf::Sprite gaugeMaskSprite;
		static float sumGauge;
		static float gaugeProgress;
		static bool isGaugeMax;
	public:
		SPGauge(){
			loadAllTexture();
			setScale(0.2f);
		};
		bool loadAllTexture();
		void setTexture();
		float getGaugeProgress(){return gaugeProgress;};
		static void resetSumGauge(){sumGauge = 0;};
		static void addGaugeProgress(){sumGauge += gaugeProgress;};
		static bool getIsGaugeMax(){return isGaugeMax;};
		sf::Sprite getSprite(){return gaugeSprite;};
		sf::Sprite getMaskSprite(){return gaugeMaskSprite;};
		void setScale(float value);
		void updateMaskSprite();
		void reset();
};