#pragma once
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include "SPManager.h"
#include <iostream>
#include "TimerUtils.h"

class BlackOut{
	private:
		sf::RenderTexture maskTexture;//暗転と透明窓を描画する場所
		sf::Sprite maskSprite;
		sf::RectangleShape fullOverlay;//暗幕
		sf::CircleShape clearCircle;//透明窓
		float currentRadius = 0.0f;
		float maxRadius;
		bool isClearing = false;
		int alpha = 180;
		sf::Vector2f centerPos;
		sf::SoundBuffer awakeBuffer;
		sf::Sound awakeSound;
		bool hasPlayedAwakeSound = false;
	public:
		BlackOut(sf::RenderWindow&);
		void startClearing(SPManager& spState);
		void circleSizeUpAnim(SPManager& spState);
		// void goDark();
		void activeSPattack(SPManager& spState);
		void draw(sf::RenderWindow& window, SPManager& spState);
};