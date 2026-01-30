// Upgrade.cpp
#include "Upgrade.h"

Upgrade::Upgrade()
	:upgradeOverlay(sf::Vector2f(800, 600)),
	 upgradeItemSize(100,100),
	 spacingUpgrade(20.0f),
	 numUpgradeItems(6),
	 upgradeItemSize(100,100),
	 posYUpgrade(250.0f),
{
	showUpgradeScreen = false;
	reviveWiggleToggle = false;
	upgradeOverlay.setFillColor(sf::Color(0, 0, 0, 150));
	totalWidthUpgrade = numUpgradeItems * upgradeItemSize.x + (numUpgradeItems - 1) * spacingUpgrade;
	startXUpgrade = (800 - totalWidthUpgrade) / 2.0f;
}


draw()
{
	for (int i = 0; i < numUpgradeItems; ++i) {
        sf::RectangleShape item(upgradeItemSize);
        float posX = startXUpgrade + i * (upgradeItemSize.x + spacingUpgrade);
        item.setPosition(posX, posYUpgrade);
        item.setFillColor(sf::Color::White);
        upgradeItems.push_back(item);
    }
    upgradeTextures.resize(numUpgradeItems);
    std::string upgradePaths[numUpgradeItems] = {
        "../image/moveSpeed.jpg",
        "../image/cleanPower.jpg",
        "../image/hpPower.jpg",
        "../image/attackPower.jpg",
        "../image/attackSpeed.jpg",
        "../image/revivalTime.jpg"
    };
    for (int i = 0; i < numUpgradeItems; ++i) {
        if (!upgradeTextures[i].loadFromFile(upgradePaths[i])) {
            std::cerr << "Failed to load " << upgradePaths[i] << std::endl;
        }
        upgradeItems[i].setTexture(&upgradeTextures[i]);
    }


    // --- 説明文のテキストオブジェクトを準備 ---
    hoverText.setFont(font);
    hoverText.setCharacterSize(20);
    hoverText.setFillColor(sf::Color::White);

    // --- 各アイコンの説明文リスト ---
    std::vector<std::string> upgradeDescriptions = {
        "Boost your speed!",
        "Clean trash faster!",
        "Max HP up!",
        "Hit harder!",
        "Attack faster!",
        "Revive quicker!"
    };

    // --- ステータス変化時の色を記録するマップ ---
    std::map<std::string, sf::Color> statChangeColors = {
        {"moveSpeedUP", sf::Color::Green},
        {"cleanSpeedUP", sf::Color::Green},
        {"maxHpUP", sf::Color::Green},
        {"attackPowerUP", sf::Color::Green},
        {"attackSpeedUP", sf::Color::Green},
        {"revivalTimeDOWN", sf::Color::Green},//ここまでメリット
        {"moveSpeedDOWN", sf::Color::Red},
        {"cleanSpeedDOWN", sf::Color::Red},
        {"maxHpDOWN", sf::Color::Red},
        {"attackPowerDOWN", sf::Color::Red},
        {"attackSpeedDOWN", sf::Color::Red},
        {"revivalTimeUP", sf::Color::Red}
    };

    //アップグレード画面のステータス関連
    sf::Text statsText;
    statsText.setFont(font);
    statsText.setCharacterSize(20);
    statsText.setFillColor(sf::Color::White);
    statsText.setPosition(500, 30); // 表示位置はお好みで

    std::vector<std::pair<std::string, float>> upcomingChanges;
}
