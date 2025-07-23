#pragma once
#include <SFML/Graphics.hpp>
#include <string>
#include <iostream>
#include "BaseUIManager.h"

class ScoreManager : public BaseUIManager{
	private:
		static int score;
	public:
		ScoreManager(std::string fontName, sf::Color fontColor, sf::RenderWindow& window, int fontSize){
			setFont(fontName);
			setColor(fontColor);
			setSize(fontSize);
			changeScore(window);
		}
		void changeScore(sf::Window&);
		int getScore(){return score;};
		static void addScore(int);
		void reset();
};