// Game.h

#pragma once
#include <SFML/Graphics.hpp>
sf::RenderWindow window(sf::VideoMode(800, 600), "Screen Transition Example");
GameState currentState = GameState::Title;
std::srand(static_cast<unsigned>(std::time(nullptr)));


// --- エネミー関連 ---
std::vector<Enemy> enemies;