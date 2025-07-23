#pragma once
#include <SFML/Graphics.hpp>
#include <vector>
#include <string>
#include <iostream>

class GaugeManager {
	public:
		GaugeManager(float x, float y, float gaugeWidth, float gaugeHeight, int maxScorePerRank, sf::RenderWindow& window);
		void loadRankTextures(const std::vector<std::string>& texturePaths);
		void updateScore(int delta);
		void draw(sf::RenderWindow& window);
		void reset();

		int getScore() const;
		int getRankIndex() const;
	private:
		float x, y;
		float gaugeWidth, gaugeHeight;
		int score;
		int maxScorePerRank;
		int rankIndex;

		sf::RectangleShape gaugeBackground;
		sf::RectangleShape gaugeBar;
		sf::Sprite rankSprite;
		std::vector<sf::Texture> rankTextures;

		void updateGaugeBar();
		void updateRankSprite();
};
