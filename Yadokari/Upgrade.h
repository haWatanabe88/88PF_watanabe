// Upgrade.h
// --- アップグレード画面 ---
#pragma once
#include <SFML/Graphics.hpp>

class Upgrade{
    public:
        Upgrade();
        draw();
    private:
        sf::RectangleShape upgradeOverlay;
        std::vector<sf::RectangleShape> upgradeItems;
        const int numUpgradeItems;
        const sf::Vector2f upgradeItemSize;
        float spacingUpgrade;
        float totalWidthUpgrade;
        float startXUpgradef;
        float posYUpgrade;
        sf::Vector2f revivalBasePosition;
        bool reviveWiggleToggle;
        bool showUpgradeScreen;
        std::vector<sf::Texture> upgradeTextures;
        sf::Text hoverText;
}


