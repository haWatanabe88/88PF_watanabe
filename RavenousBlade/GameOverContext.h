#pragma once
#include <SFML/Graphics.hpp>
#include "Button.h"
#include "GaugeManager.h"
#include <string>
#include "GameContext.h"

struct GameOverContext
{
	enum class GaugeState{
		normal,
		up,
		down
	};
	GaugeState curState;
    sf::Text gameOverText;
    sf::Text scoreText;
	sf::Font font;
	Button titleBtn;
    sf::Texture texture_p;
	sf::Texture texture_g;
	sf::Texture texture_s;
	sf::Texture texture_b;
    sf::Sprite spriteMedal;
	sf::RectangleShape fullOverlay;//暗幕
	sf::Clock progressClock;
	float time;
	float th;
	int curScore;
	GaugeManager gauge;

	GameOverContext(sf::RenderWindow&, GameContext&);
	void draw(sf::RenderWindow&, int);
	bool setFont(std::string);
	void setSize(sf::Text&, int);
	void update(sf::RenderWindow&);
	void scoreUpdate(sf::RenderWindow&, sf::Text, int, int&);
	void reset();
};
