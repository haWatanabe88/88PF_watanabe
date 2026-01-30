#pragma once
#include <SFML/Graphics.hpp>
#include "Button.h"
#include <string>
#include "GameContext.h"

struct GameTitleContext{
	sf::Text titleText;
	Button startBtn;
	sf::Font font;

	GameTitleContext(sf::RenderWindow&, GameContext&);
	bool setFont(std::string);
	void setSize(sf::Text&, int);
	void draw(sf::RenderWindow&);
	void update(sf::RenderWindow&);
};

