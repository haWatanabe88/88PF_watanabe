#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>

class BaseUIManager{
	public:
		bool setFont(std::string);
		void setSize(int);
		void setColor(sf::Color);
		sf::Text getText(){return text;};
		sf::Text getText2(){return text2;};
		virtual void customTextPos(sf::Text& text, sf::Window& window, float margin_x, float margin_y);
	protected:
		sf::Text text;
		sf::Text text2;
		sf::Font font;
};