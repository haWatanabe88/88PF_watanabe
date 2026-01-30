#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include "BaseUIManager.h"

class ComboManager : public BaseUIManager{
	public:
		ComboManager(std::string fontName, sf::Color fontColor, sf::RenderWindow& window, int fontSize){
			setFont(fontName);
			setColor(fontColor);
			setSize(fontSize);
			changeComboGrade(window, comboCounter);
		}
		enum class ComboRank{
			normal,
			good,
			great,
			wonderful,
			marvelous,
			genius,
		};
		ComboRank getComboRank(){return comboRank;};
		void changeComboRank(int);
		void resetComboRank(){comboRank = ComboRank::normal;};
		static float getComboMultiplier(){return comboMultiplier;};
		void changeComboMultiplier(ComboRank);
		static void resetComboCount(){comboCounter = 0;};
		static void addComboCounter(){comboCounter++;};
		void changeComboGrade(sf::Window& window, int);
		void changeComboCounter(sf::Window& window);
		int getComboCounter(){return comboCounter;};
		void reset();
	private:
		ComboRank comboRank = ComboRank::normal;
		static int comboCounter;
		static float comboMultiplier;
};