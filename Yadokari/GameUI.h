// GameUI.h
#pragma once
#include <SFML/Graphics.hpp>
#include "GameState.h"

// --- タイトル画面：ボタン ---
sf::RectangleShape button(sf::Vector2f(200, 100));
button.setPosition(300, 250);
button.setFillColor(sf::Color::Blue);

// --- フォント ---
sf::Font font;
if (!font.loadFromFile("/System/Library/Fonts/Supplemental/Arial.ttf")) {
	std::cerr << "Failed to load font." << std::endl;
	return -1;
}
sf::Text buttonText("Game Start", font, 30);
buttonText.setFillColor(sf::Color::White);
sf::FloatRect btnTextBounds = buttonText.getLocalBounds();
buttonText.setOrigin(btnTextBounds.left + btnTextBounds.width / 2.0f,
						btnTextBounds.top + btnTextBounds.height / 2.0f);
buttonText.setPosition(
	button.getPosition().x + button.getSize().x / 2.0f,
	button.getPosition().y + button.getSize().y / 2.0f
);

sf::RectangleShape expBarBackground(sf::Vector2f(300, 20));
expBarBackground.setPosition(500, 20);
expBarBackground.setFillColor(sf::Color(50, 50, 50));
sf::RectangleShape expBar(sf::Vector2f(0, 20));
expBar.setPosition(500, 20);
expBar.setFillColor(sf::Color(0, 255, 0));


// --- Clear画面 ---
sf::Text clearText;
clearText.setFont(font);
clearText.setString("Clear!");
clearText.setCharacterSize(60);
clearText.setFillColor(sf::Color::Green);
sf::FloatRect clearTextBounds = clearText.getLocalBounds();
clearText.setOrigin(clearTextBounds.width / 2.0f, clearTextBounds.height / 2.0f);
clearText.setPosition(400, 300);

// --- GameOver画面 ---
sf::Text gameOverText;
gameOverText.setFont(font);
gameOverText.setString("GameOver...");
gameOverText.setCharacterSize(60);
gameOverText.setFillColor(sf::Color::Red);
sf::FloatRect gameOverTextBounds = gameOverText.getLocalBounds();
gameOverText.setOrigin(gameOverTextBounds.width / 2.0f, gameOverTextBounds.height / 2.0f);
gameOverText.setPosition(400, 300);

// --- Restartボタン ---
sf::RectangleShape restartButton(sf::Vector2f(200, 60));
restartButton.setFillColor(sf::Color::Green);
restartButton.setPosition(300, 400);

sf::Text restartButtonText("Restart", font, 30);
restartButtonText.setFillColor(sf::Color::White);
sf::FloatRect restartTextBounds = restartButtonText.getLocalBounds();
restartButtonText.setOrigin(restartTextBounds.left + restartTextBounds.width / 2.0f,
							restartTextBounds.top + restartTextBounds.height / 2.0f);
restartButtonText.setPosition(
	restartButton.getPosition().x + restartButton.getSize().x / 2.0f,
	restartButton.getPosition().y + restartButton.getSize().y / 2.0f
);

// --- プレイヤーHPバー ---
sf::RectangleShape playerHpBarBackground(sf::Vector2f(64.0f, 8.0f));
playerHpBarBackground.setFillColor(sf::Color(100, 100, 100));
sf::RectangleShape playerHpBar(sf::Vector2f(64.0f, 8.0f));
playerHpBar.setFillColor(sf::Color::Green);

    // 初回生成時にも汚染ゲージを増加
    float pollution = 0.0f;
    const float pollutionIncrement = 5.0f;
    const float pollutionMax = 100.0f;
    pollution = std::min(pollution + pollutionIncrement, pollutionMax);

    // --- 汚染ゲージ ---
    sf::RectangleShape pollutionBarBackground(sf::Vector2f(30, 300));
    pollutionBarBackground.setPosition(20, 20);
    pollutionBarBackground.setFillColor(sf::Color(100, 100, 100));
    sf::RectangleShape pollutionBarFill(sf::Vector2f(30, 0));
    pollutionBarFill.setPosition(20, 20 + 300);
    pollutionBarFill.setFillColor(sf::Color(0, 0, 255));
