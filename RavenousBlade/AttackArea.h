#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include "Enemy.h"
#include "SPManager.h"

class AttackArea{
	protected:
		sf::CircleShape circle;
		float r;
		sf::Color orgColor;
		bool isContact = false;
		float maxRadius;
	public:
		AttackArea(float, sf::Color, sf::RenderWindow&, std::vector<AttackArea*>&);
		void setColor(sf::Color);
		virtual void checkContact(Enemy& e) = 0;
		void gainScore();
		void resetCheckContact();
		sf::Vector2f getCenterPos(sf::RenderWindow& window);
		void setPosition(sf::Vector2f);
		virtual void showSprite(sf::RenderWindow& window, SPManager&);
		float getRadius(){return r;};
		sf::Vector2f getPosition();
		float calcDistance(Enemy& e);
};