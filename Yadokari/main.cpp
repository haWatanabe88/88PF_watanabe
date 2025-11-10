#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <cstdlib>
#include <ctime>
#include <cmath>
#include <algorithm>
#include <sstream>
#include <map>


// 画面状態を示す列挙型
enum class GameState {
    Title,  // タイトル画面
    Game,   // ゲームプレイ画面
    Clear,  // ゲームクリア画面
    GameOver // 汚染ゲージ上限到達時の画面（GameOver... 赤文字）
};

// プレイヤー（girlSprite）の能力をまとめる構造体
struct PlayerStats {
    float moveSpeed;    // 移動速度
    float cleanSpeed;   // 掃除スピード（衝突後の遅延時間）
    int hp;             // 現在のHP
    int maxHp;          // 初期/最大HP
    int attackPower;    // 攻撃力
    float attackSpeed;  // 攻撃間隔（プレイヤーの攻撃間隔）
    float revivalTime;  // 復活時間
};

// エネミーの能力をまとめる構造体
struct EnemyStats {
    float moveSpeed;
    int hp;
    int attackPower;
    float attackSpeed;  // エネミーの攻撃間隔
};

// エネミーを管理する構造体（スプライトと能力）
struct Enemy {
    sf::Sprite sprite;
    EnemyStats stats;
};

void resetGameState(
    GameState& currentState,
    sf::Time& activeTime,
    sf::Clock& gameClock,
    sf::Clock& spawnClock,
    sf::Clock& enemySpawnClock,
    float& experience,
    int& upgradeCount,
    float& getexperience,
    float& pollution,
    PlayerStats& player,
    bool& isReviving,
    std::vector<Enemy>& enemies,
    std::vector<sf::Sprite>& trushSprites,
    int& collidedEnemyIndex,
    bool& enemyCollisionPending,
    bool& collisionPending,
    bool& imageLoaded,
    sf::Texture& trushTexture
) {
    currentState = GameState::Title;
    activeTime = sf::Time::Zero;
    gameClock.restart();
    spawnClock.restart();
    enemySpawnClock.restart();
    experience = 0.0f;
    upgradeCount = 0;
    getexperience = 50.0f;
    pollution = 0.0f;
    player.moveSpeed = 0.01f;
    player.cleanSpeed = 2.0f;
    player.hp = 100;
    player.maxHp = 100;
    player.attackPower = 10;
    player.attackSpeed = 2.0f;
    player.revivalTime = 5.0f;
    isReviving = false;
    enemies.clear();
    trushSprites.clear();
    collidedEnemyIndex = -1;
    enemyCollisionPending = false;
    collisionPending = false;
    imageLoaded = false;

    // 初期ゴミを1個だけ出しておく
    sf::Sprite ts;
    ts.setTexture(trushTexture);
    ts.setOrigin(ts.getLocalBounds().width / 2.0f, ts.getLocalBounds().height / 2.0f);
    ts.setScale(0.3f, 0.3f);
    ts.setPosition(static_cast<float>(std::rand() % 700 + 50), static_cast<float>(std::rand() % 500 + 50));
    trushSprites.push_back(ts);
}

// --- ステータスの上限・下限定義（レベルデザイン用） ---
//メリット
const float MOVE_SPEED_MAX = 0.08f;
const float CLEAN_SPEED_MIN = 0.2f;
const int MAX_HP_MAX = 300;
const int ATTACK_POWER_MAX = 100;
const float ATTACK_SPEED_MIN = 0.3f;
const float REVIVAL_TIME_MIN = 0.1f;

//デメリット
const float MOVE_SPEED_MIN = 0.001f;
const float CLEAN_SPEED_MAX = 4.0f;
const int MAX_HP_MIN = 10;
const int ATTACK_POWER_MIN = 1;
const float ATTACK_SPEED_MAX = 5.0f;
const float REVIVAL_TIME_MAX = 10.0f;

// --- ステータス変更量（後で調整しやすい） ---
//メリット
const float MOVE_SPEED_UP = 0.01f;
const float CLEAN_SPEED_UP = -0.5f;
const int MAX_HP_UP = 50;
const int ATTACK_POWER_UP = 15;
const float ATTACK_SPEED_UP = -0.5f;
const float REVIVAL_TIME_DOWN = -1.0f;

//デメリット
const float MOVE_SPEED_DOWN = -0.003f;
const float CLEAN_SPEED_DOWN = 0.2f;
const int MAX_HP_DOWN = -20;
const int ATTACK_POWER_DOWN = -2;
const float ATTACK_SPEED_DOWN = 0.3f;
const float REVIVAL_TIME_UP = 0.3f;

// --- ほぼ等しいか判定するユーティリティ関数 ---
auto isNearlyEqual = [](float a, float b, float precision = 0.001f) {
    return std::abs(a - b) <= precision;
};

int main() {
    sf::RenderWindow window(sf::VideoMode(800, 600), "Screen Transition Example");
    GameState currentState = GameState::Title;
    std::srand(static_cast<unsigned>(std::time(nullptr)));

    // --- プレイヤー能力の初期化 ---
    PlayerStats player = {
        0.01f,  // moveSpeed
        2.0f,   // cleanSpeed
        100,    // hp (初期値)
        100,    // maxHp
        10,     // attackPower
        2.0f,   // attackSpeed
        5.0f    // revivalTime
    };

    // --- プレイヤーHPバー ---
    sf::RectangleShape playerHpBarBackground(sf::Vector2f(64.0f, 8.0f));
    playerHpBarBackground.setFillColor(sf::Color(100, 100, 100));
    sf::RectangleShape playerHpBar(sf::Vector2f(64.0f, 8.0f));
    playerHpBar.setFillColor(sf::Color::Green);

    // 復活処理管理用
    bool isReviving = false;
    sf::Clock revivalClock;

    // --- ゴミ（trush）関連 ---
    std::vector<sf::Sprite> trushSprites;
    sf::Texture trushTexture;
    if (!trushTexture.loadFromFile("../image/trush.png")) {
        std::cerr << "Failed to load trush.jpg" << std::endl;
        return -1;
    }
    float trushSpawnInterval = 3.0f;///変更
    size_t maxTrushCount = 100;//変更
    sf::Clock trushSpawnClock;
    sf::Clock spawnClock;
    sf::Time spawnTimeAcc = sf::Time::Zero;
    sf::Sprite ts;
    ts.setTexture(trushTexture);
    sf::FloatRect tsBounds = ts.getLocalBounds();
    ts.setOrigin(ts.getLocalBounds().width / 2.0f, ts.getLocalBounds().height / 2.0f);
    ts.setScale(0.07f, 0.07f);
    float randX = static_cast<float>(std::rand() % 700 + 50);
    float randY = static_cast<float>(std::rand() % 500 + 50);
    ts.setPosition(randX, randY);
    trushSprites.push_back(ts);

    // 初回生成時にも汚染ゲージを増加
    float pollution = 0.0f;
    const float pollutionIncrement = 5.0f;
    const float pollutionMax = 100.0f;
    pollution = std::min(pollution + pollutionIncrement, pollutionMax);

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

    // --- タイマー ---
    sf::Text timerText;
    timerText.setFont(font);
    timerText.setCharacterSize(40);
    timerText.setFillColor(sf::Color::Red);
    timerText.setPosition(250, 20);
    sf::Clock gameClock;
    sf::Time activeTime = sf::Time::Zero;
    const sf::Time timeLimit = sf::seconds(300);//////////////////ゲーム時間

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

    sf::Texture hermitCrabTexture;
    const int frameWidth = 235;//64_64
    const int frameHeight = 200;
    const int numFrames = 4;
    int currentFrame = 0;
    float animationSpeed = 0.2f;  // アニメーションスピード（秒）
    sf::Clock animationClock;
    sf::Sprite girlSprite;
    bool imageLoaded = false;

    // --- 経験値 ---
    float experience = 0.0f;
    float getexperience = 50.0f;
    float getexperiencedefeatenemy = 30.0f;

    const float maxExperience = 100.0f;
    sf::RectangleShape expBarBackground(sf::Vector2f(300, 20));
    expBarBackground.setPosition(500, 20);
    expBarBackground.setFillColor(sf::Color(50, 50, 50));
    sf::RectangleShape expBar(sf::Vector2f(0, 20));
    expBar.setPosition(500, 20);
    expBar.setFillColor(sf::Color(0, 255, 0));
    int upgradeCount = 0;  // アップグレードされた回数を記録する

    // --- 汚染ゲージ ---
    sf::RectangleShape pollutionBarBackground(sf::Vector2f(30, 300));
    pollutionBarBackground.setPosition(20, 20);
    pollutionBarBackground.setFillColor(sf::Color(100, 100, 100));
    sf::RectangleShape pollutionBarFill(sf::Vector2f(30, 0));
    pollutionBarFill.setPosition(20, 20 + 300);
    pollutionBarFill.setFillColor(sf::Color(0, 0, 255));

    // --- アップグレード画面 ---
    bool showUpgradeScreen = false;
    sf::RectangleShape upgradeOverlay(sf::Vector2f(800, 600));
    upgradeOverlay.setFillColor(sf::Color(0, 0, 0, 150));
    std::vector<sf::RectangleShape> upgradeItems;
    const int numUpgradeItems = 6;
    const sf::Vector2f upgradeItemSize(100, 100);
    float spacingUpgrade = 20.0f;
    float totalWidthUpgrade = numUpgradeItems * upgradeItemSize.x + (numUpgradeItems - 1) * spacingUpgrade;
    float startXUpgrade = (800 - totalWidthUpgrade) / 2.0f;
    float posYUpgrade = 250.0f;
    sf::Vector2f revivalBasePosition;
    bool reviveWiggleToggle = false;
    for (int i = 0; i < numUpgradeItems; ++i) {
        sf::RectangleShape item(upgradeItemSize);
        float posX = startXUpgrade + i * (upgradeItemSize.x + spacingUpgrade);
        item.setPosition(posX, posYUpgrade);
        item.setFillColor(sf::Color::White);
        upgradeItems.push_back(item);
    }
    std::vector<sf::Texture> upgradeTextures;
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
    sf::Text hoverText;
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

    // --- エネミー関連 ---
    std::vector<Enemy> enemies;
    // --- enemySprite 用アニメーション設定 ---
    sf::Texture enemySheetTexture;
    if (!enemySheetTexture.loadFromFile("../image/enemy1.png")) {
        std::cerr << "Failed to load enemy1.png" << std::endl;
        return -1;
    }
    const int enemyFrameCount = 4;
    const int enemyFrameWidth = 260;
    const int enemyFrameHeight = 250;
    int currentEnemyFrame = 0;
    sf::Clock enemyAnimationClock;
    float enemyAnimationSpeed = 0.2f; // 秒

    float enemySpawnInterval = 5.0f;
    const size_t enemyMaxCount = 3;
    sf::Clock enemySpawnClock;
    bool enemyCollisionPending = false;
    sf::Clock enemyCollisionClock;
    int collidedEnemyIndex = -1;
    float enemyCollisionPauseTime = 0.5f;
    sf::Clock playerAttackClock;
    sf::Clock enemyAttackClock;

    // --- 衝突（掃除）遅延処理 ---
    bool collisionPending = false;
    sf::Clock collisionClock;
    int collidedTrushIndex = -1;

    // --- メインループ ---
    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed)
                window.close();

// --- プレイヤー能力の初期化 ---
// PlayerStats player = {
//     0.01f,  // moveSpeed max:
//     2.0f,   // cleanSpeed min:
//     100,    // hp (初期値) min:
//     100,    // maxHp min:
//     10,     // attackPower min:
//     2.0f,   // attackSpeed min:
//     5.0f    // revivalTime min:
// };
// newEnemy.stats.moveSpeed = 0.01f;
// newEnemy.stats.hp = 50;
// newEnemy.stats.attackPower = 10;
// newEnemy.stats.attackSpeed = 1.0f;
            // --- アップグレード画面中のクリック処理 ---
            if (showUpgradeScreen) {
                if (event.type == sf::Event::MouseButtonPressed && event.mouseButton.button == sf::Mouse::Left) {
                    sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                    for (int i = 0; i < upgradeItems.size(); ++i) {
                        if (upgradeItems[i].getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
                            std::string fileName = upgradePaths[i];
                            upcomingChanges.clear();
        
                            if (fileName.find("moveSpeed.jpg") != std::string::npos) {
                                player.moveSpeed = std::min(player.moveSpeed + MOVE_SPEED_UP, MOVE_SPEED_MAX);
                                player.revivalTime = std::min(player.revivalTime + REVIVAL_TIME_UP, REVIVAL_TIME_MAX);
                                upcomingChanges.push_back({"moveSpeed", MOVE_SPEED_UP});
                                upcomingChanges.push_back({"revivalTime", REVIVAL_TIME_UP});
                            }
                            if (fileName.find("cleanPower.jpg") != std::string::npos) {
                                player.cleanSpeed = std::max(player.cleanSpeed + CLEAN_SPEED_UP, CLEAN_SPEED_MIN);
                                player.attackPower = std::max(player.attackPower + ATTACK_POWER_DOWN, ATTACK_POWER_MIN);
                                upcomingChanges.push_back({"cleanSpeed", CLEAN_SPEED_UP});
                                upcomingChanges.push_back({"attackPower", ATTACK_POWER_DOWN});
                            }
                            if (fileName.find("hpPower.jpg") != std::string::npos) {
                                player.maxHp = std::min(player.maxHp + MAX_HP_UP, MAX_HP_MAX);
                                player.hp += MAX_HP_UP;/////////////////////////////////////////hpを加算している
                                player.moveSpeed = std::max(player.moveSpeed + MOVE_SPEED_DOWN, MOVE_SPEED_MIN);
                                upcomingChanges.push_back({"maxHp", MAX_HP_UP});
                                upcomingChanges.push_back({"moveSpeedDown", MOVE_SPEED_DOWN});
                            }
                            if (fileName.find("attackPower.jpg") != std::string::npos) {
                                player.attackPower = std::min(player.attackPower + ATTACK_POWER_UP, ATTACK_POWER_MAX);
                                player.attackSpeed = std::min(player.attackSpeed + ATTACK_SPEED_DOWN, ATTACK_SPEED_MAX);
                                upcomingChanges.push_back({"attackPower", ATTACK_POWER_UP});
                                upcomingChanges.push_back({"attackSpeedUp", ATTACK_SPEED_DOWN});
                            }
                            if (fileName.find("attackSpeed.jpg") != std::string::npos) {
                                player.attackSpeed = std::max(player.attackSpeed + ATTACK_SPEED_UP, ATTACK_SPEED_MIN);
                                player.cleanSpeed = std::min(player.cleanSpeed + CLEAN_SPEED_DOWN, CLEAN_SPEED_MAX);
                                upcomingChanges.push_back({"attackSpeed", ATTACK_SPEED_UP});
                                upcomingChanges.push_back({"cleanSpeedUp", CLEAN_SPEED_DOWN});
                            }
                            if (fileName.find("revivalTime.jpg") != std::string::npos) {
                                player.revivalTime = std::max(player.revivalTime + REVIVAL_TIME_DOWN, REVIVAL_TIME_MIN);
                                player.maxHp = std::max(player.maxHp + MAX_HP_DOWN, MAX_HP_MIN);
                                if(player.hp > player.maxHp)
                                {
                                    player.hp = player.maxHp;
                                }
                                upcomingChanges.push_back({"revivalTime", REVIVAL_TIME_DOWN});
                                upcomingChanges.push_back({"maxHpDown", MAX_HP_DOWN});
                            }
                            showUpgradeScreen = false;
                            gameClock.restart();
                            break;
                        }
                    }
                }
                continue;
            }

            
            // タイトル画面処理
            if (currentState == GameState::Title) {
                if (event.type == sf::Event::MouseButtonPressed && event.mouseButton.button == sf::Mouse::Left) {
                    sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                    if (button.getGlobalBounds().contains(static_cast<float>(mousePos.x),
                                                            static_cast<float>(mousePos.y))) {
                        std::cout << "Button Clicked! Transitioning to Game Screen." << std::endl;
                        currentState = GameState::Game;
                        if (!imageLoaded) {
                            if (!hermitCrabTexture.loadFromFile("../image/hermitcrab_spritesheet.png")) {
                                std::cerr << "Failed to load hermitcrab_spritesheet.jpg" << std::endl;
                                return -1;
                            }
                            girlSprite.setTexture(hermitCrabTexture);
                            girlSprite.setTextureRect(sf::IntRect(0, 0, frameWidth, frameHeight));
                            girlSprite.setOrigin(frameWidth / 2.0f, frameHeight / 2.0f);
                            girlSprite.setScale(0.3f, 0.3f);  // スケーリング調整
                            girlSprite.setPosition(400, 300);
                            imageLoaded = true;
                        }
                        activeTime = sf::Time::Zero;
                        gameClock.restart();
                        spawnClock.restart();
                        enemySpawnClock.restart();
                    }
                }
            }
            // --- Restartボタン処理 ---
            if ((currentState == GameState::GameOver || currentState == GameState::Clear) &&
                event.type == sf::Event::MouseButtonPressed && event.mouseButton.button == sf::Mouse::Left) {
                
                sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                if (restartButton.getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
                    std::cout << "Restarting game..." << std::endl;
                
                    resetGameState(currentState, activeTime, gameClock, spawnClock, enemySpawnClock,
                                   experience, upgradeCount, getexperience, pollution,
                                   player, isReviving, enemies, trushSprites,
                                   collidedEnemyIndex, enemyCollisionPending, collisionPending,
                                   imageLoaded, trushTexture);
                }
            }
        } // イベント処理終了

        window.clear(sf::Color::Black);

        if (currentState == GameState::Title) {
            window.draw(button);
            window.draw(buttonText);
        }
        else if (currentState == GameState::Game) {
            if (!showUpgradeScreen) {// && !isReviving
                sf::Time deltaTime = gameClock.restart();
                activeTime += deltaTime;
                // --- アニメーション更新処理（GameState::Game の中の更新ブロック内） ---
                if (!collisionPending && !enemyCollisionPending && !trushSprites.empty() && !isReviving) {
                    if (animationClock.getElapsedTime().asSeconds() >= animationSpeed) {
                        currentFrame = (currentFrame + 1) % numFrames;
                        girlSprite.setTextureRect(sf::IntRect(currentFrame * frameWidth + 50 , 400, frameWidth, frameHeight));
                        animationClock.restart();
                    }
                }
                if (enemyAnimationClock.getElapsedTime().asSeconds() >= enemyAnimationSpeed) {
                    currentEnemyFrame = (currentEnemyFrame + 1) % enemyFrameCount;
                    for (auto& enemy : enemies) {
                        enemy.sprite.setTextureRect(sf::IntRect(currentEnemyFrame * enemyFrameWidth, 400, enemyFrameWidth, enemyFrameHeight));
                    }
                    enemyAnimationClock.restart();
                }
                // --- ゴミ生成 ---
                sf::Time spawnDelta = spawnClock.restart();
                spawnTimeAcc += spawnDelta;
                if (spawnTimeAcc.asSeconds() >= trushSpawnInterval) {
                    if (trushSprites.size() < maxTrushCount) {
                        sf::Sprite newTrush;
                        newTrush.setTexture(trushTexture);
                        newTrush.setScale(0.07f, 0.07f);
                        newTrush.setOrigin(ts.getLocalBounds().width / 2.0f, ts.getLocalBounds().height / 2.0f);
                        float randX = static_cast<float>(std::rand() % 700 + 50);
                        float randY = static_cast<float>(std::rand() % 500 + 50);
                        newTrush.setPosition(randX, randY);
                        trushSprites.push_back(newTrush);
                        pollution = std::min(pollution + pollutionIncrement, pollutionMax);
                        if (pollution >= pollutionMax) {
                            std::cout << "Pollution reached maximum. Game Over." << std::endl;
                            currentState = GameState::GameOver;
                        }
                    }
                    spawnTimeAcc -= sf::seconds(trushSpawnInterval);
                }
                
                // --- タイマー更新 ---
                sf::Time remaining = timeLimit - activeTime;
                if (remaining <= sf::Time::Zero) {
                    currentState = GameState::Clear;
                } else {
                    int secondsLeft = static_cast<int>(remaining.asSeconds());
                    timerText.setString("Time: " + std::to_string(secondsLeft));
                    window.draw(timerText);
                }
                
                // --- エネミー生成 ---
                if (enemySpawnClock.getElapsedTime().asSeconds() >= enemySpawnInterval && !isReviving) {//&& !isRevivingによってエネミー生成は停止
                    if (enemies.size() < enemyMaxCount) {
                        Enemy newEnemy;
                        newEnemy.sprite.setTexture(enemySheetTexture);
                        newEnemy.sprite.setTextureRect(sf::IntRect(30, 380, enemyFrameWidth, enemyFrameHeight));
                        newEnemy.sprite.setScale(0.3f, 0.3f); // 必要に応じて調整
                        sf::FloatRect enemyBounds = newEnemy.sprite.getLocalBounds();
                        newEnemy.sprite.setOrigin(enemyBounds.width / 2.0f, enemyBounds.height / 2.0f);
                        int edge = std::rand() % 4;
                        if (edge == 0)
                            newEnemy.sprite.setPosition(static_cast<float>(std::rand() % 800), 0);
                        else if (edge == 1)
                            newEnemy.sprite.setPosition(static_cast<float>(std::rand() % 800), 600);
                        else if (edge == 2)
                            newEnemy.sprite.setPosition(0, static_cast<float>(std::rand() % 600));
                        else
                            newEnemy.sprite.setPosition(800, static_cast<float>(std::rand() % 600));
                        newEnemy.stats.moveSpeed = 0.01f;
                        newEnemy.stats.hp = 50;
                        newEnemy.stats.attackPower = 10;
                        newEnemy.stats.attackSpeed = 1.0f;
                        enemies.push_back(newEnemy);
                    }
                    enemySpawnClock.restart();
                }
                
                // --- エネミー移動 ---
                if (!enemyCollisionPending && !isReviving) {/////////////&& !isRevivingによってエネミーの移動も停止
                    for (size_t i = 0; i < enemies.size(); ++i) {
                        sf::Vector2f enemyPos = enemies[i].sprite.getPosition();
                        sf::Vector2f girlPos = girlSprite.getPosition();
                        sf::Vector2f dir = girlPos - enemyPos;
                        float len = std::sqrt(dir.x * dir.x + dir.y * dir.y);
                        if (dir.x < 0) {
                            enemies[i].sprite.setScale(-0.3f, 0.3f);  // 左向き（左右反転）
                        } else if (dir.x > 0) {
                             enemies[i].sprite.setScale(0.3f, 0.3f);   // 右向き（通常）
                        }
                        if (len > 0.1f) {
                            dir /= len;
                            enemies[i].sprite.move(dir * enemies[i].stats.moveSpeed);
                        }
                    }
                }
                
                // --- エネミーと girlSprite の衝突判定 ---
                if (!enemyCollisionPending) {
                    for (size_t i = 0; i < enemies.size(); ++i) {
                        if (girlSprite.getGlobalBounds().intersects(enemies[i].sprite.getGlobalBounds())) {
                            enemyCollisionPending = true;
                            collidedEnemyIndex = i;
                            enemyCollisionClock.restart();
                            playerAttackClock.restart();
                            enemyAttackClock.restart();
                            // ここで girlSprite も動かないようにするため、移動処理は上で条件分岐済み
                            break;
                        }
                    }
                }
                if (enemyCollisionPending) {
                    // プレイヤー攻撃処理：player.attackSpeed 間隔ごとに攻撃
                    if (playerAttackClock.getElapsedTime().asSeconds() >= player.attackSpeed) {
                        std::cout << "Player attacks enemy." << std::endl;
                        std::cout<<playerAttackClock.getElapsedTime().asSeconds()<<std::endl;
                        enemies[collidedEnemyIndex].stats.hp -= player.attackPower;
                        playerAttackClock.restart();
                    }
                    // エネミー攻撃処理：enemy.attackSpeed 間隔ごとに攻撃
                    if (enemyAttackClock.getElapsedTime().asSeconds() >= enemies[collidedEnemyIndex].stats.attackSpeed) {
                        std::cout << "Enemy attacks player." << std::endl;
                        player.hp -= enemies[collidedEnemyIndex].stats.attackPower;
                        enemyAttackClock.restart();
                    }
                    // 衝突待機解除：HP条件のみで解除（あなたのご指摘通り）
                    if (enemies[collidedEnemyIndex].stats.hp <= 0) {
                        std::cout << "エネミーやられた" << std::endl;
                        experience += getexperiencedefeatenemy;
                        enemies.erase(enemies.begin() + collidedEnemyIndex);
                        collidedEnemyIndex = -1;
                        enemyCollisionPending = false;
                        if (experience >= maxExperience) {/////的撃破時に経験値アップを考慮
                            std::cout << "Experience reached maximum. Showing upgrade screen." << std::endl;
                            experience = experience - maxExperience;
                            showUpgradeScreen = true;
                        }
                    }
                    if (player.hp <= 0) {
                        // 復活処理に移行する
                        std::cout<<"やられた"<<std::endl;
                        // また、もし girlSprite と衝突中のエネミーが存在すれば削除する
                        for (size_t i = 0; i < enemies.size(); ) {
                            if (girlSprite.getGlobalBounds().intersects(enemies[i].sprite.getGlobalBounds())) {
                                enemies.erase(enemies.begin() + i);
                            } else {
                                ++i;
                            }
                        }
                        isReviving = true;
                        revivalBasePosition = girlSprite.getPosition();  // 初期位置を保持
                        enemyCollisionPending = false;
                        revivalClock.restart();
                    }
                }

                // --- girlSprite の移動（最も近い trush へ向かう） ---
                if (!collisionPending && !enemyCollisionPending && !trushSprites.empty() && !isReviving) {
                    sf::Vector2f girlPos = girlSprite.getPosition();
                    float minDistance = std::numeric_limits<float>::max();
                    size_t closestIndex = 0;

                    for (size_t i = 0; i < trushSprites.size(); ++i) {
                        sf::Vector2f trushPos = trushSprites[i].getPosition();
                        sf::Vector2f delta = trushPos - girlPos;
                        float dist = delta.x * delta.x + delta.y * delta.y;  // 一時的にスクエア距離の二乗で比較
                        if (dist < minDistance) {
                            minDistance = dist;
                            closestIndex = i;
                        }
                    }

                    sf::Vector2f targetPos = trushSprites[closestIndex].getPosition();
                    sf::Vector2f direction = targetPos - girlPos;
                    float length = std::sqrt(direction.x * direction.x + direction.y * direction.y);
                    if (length > 0.1f) {
                        direction /= length;
                        girlSprite.move(direction * player.moveSpeed);
                    }
                    if (direction.x < 0) {
                        girlSprite.setScale(-0.3f, 0.3f);  // 左向き（左右反転）
                    } else if (direction.x > 0) {
                        girlSprite.setScale(0.3f, 0.3f);   // 右向き（通常）
                    }
                }
                
                // --- ゴミとの衝突判定 ---
                if (!collisionPending && !trushSprites.empty()) {
                    sf::Vector2f girlPos = girlSprite.getPosition();
                    size_t nearestIndex = 0;
                    float minDistance = std::numeric_limits<float>::max();
                
                    for (size_t i = 0; i < trushSprites.size(); ++i) {
                        sf::Vector2f trushPos = trushSprites[i].getPosition();
                        float dx = girlPos.x - trushPos.x;
                        float dy = girlPos.y - trushPos.y;
                        float distance = std::sqrt(dx * dx + dy * dy);
                        if (distance < minDistance) {
                            minDistance = distance;
                            nearestIndex = i;
                        }
                    }
                
                    if (girlSprite.getGlobalBounds().intersects(trushSprites[nearestIndex].getGlobalBounds())) {
                        collisionPending = true;
                        collidedTrushIndex = nearestIndex;
                        collisionClock.restart();
                    }
                }
                if (collisionPending) {
                    if (collisionClock.getElapsedTime().asSeconds() >= player.cleanSpeed) {
                        experience += getexperience;
                        trushSprites.erase(trushSprites.begin() + collidedTrushIndex);
                        pollution = std::max(pollution - pollutionIncrement, 0.0f);
                        collisionPending = false;
                        if (experience >= maxExperience) {
                            std::cout << "Experience reached maximum. Showing upgrade screen." << std::endl;
                            experience = experience - maxExperience;
                            ++upgradeCount;
                            getexperience = std::max(10.0f, 50.0f / (1.0f + upgradeCount * 0.5f)); // ←ここがキモ！
                            showUpgradeScreen = true;
                        }
                    }
                }
                // --- 経験値バー更新 ---
                float expBarWidth = 300 * (experience / maxExperience);
                expBar.setSize(sf::Vector2f(expBarWidth, 20));

                // --- 復活処理（girlSprite HP が0の場合） ---
                // 復活中は girlSprite の更新を停止し、エネミー生成・移動も停止
                // ただし、ゴミ生成は続ける
                sf::Vector2f currentScale = girlSprite.getScale();
                if (isReviving) {
                    girlSprite.setScale(currentScale.x, -std::abs(currentScale.y)); // Y軸反転
                    float elapsed = revivalClock.getElapsedTime().asSeconds();
                    std::cout<<revivalClock.getElapsedTime().asSeconds()<<std::endl;
                    if (std::fmod(elapsed, 0.5f) < 0.01f) {
                        float offset = reviveWiggleToggle ? -3.0f : 3.0f;
                        girlSprite.setPosition(revivalBasePosition.x, revivalBasePosition.y + offset);
                        reviveWiggleToggle = !reviveWiggleToggle;
                    }
                    if (revivalClock.getElapsedTime().asSeconds() >= player.revivalTime) {
                        // 復活終了時：girlSprite の HP をリセット
                        player.hp = player.maxHp;
                        isReviving = false;
                        girlSprite.setScale(currentScale.x, std::abs(currentScale.y));  // Y軸通常
                        girlSprite.setPosition(revivalBasePosition); // 元の位置に戻す
                    }
                }else {
                    girlSprite.setScale(currentScale.x, std::abs(currentScale.y));  // Y軸通常
                }

                // --- プレイヤーのHPバー更新 ---
                float hpRatio = 0.0f;
                if (isReviving) {
                    float elapsed = revivalClock.getElapsedTime().asSeconds();
                    hpRatio = std::min(elapsed / player.revivalTime, 1.0f);  // 復活アニメーション中は時間で比率
                } else {
                    hpRatio = static_cast<float>(player.hp) / static_cast<float>(player.maxHp);  // 通常時は現在HPから比率計算
                }

                float barWidth = 64.0f;
                float barHeight = 8.0f;
                playerHpBar.setSize(sf::Vector2f(barWidth * hpRatio, barHeight));

                // girlSpriteの上にバーを配置（frameHeight を使って位置調整）
                sf::Vector2f girlPos = girlSprite.getPosition();
                float girlOffsetY = frameHeight * 0.2f;
                playerHpBarBackground.setPosition(girlPos.x - barWidth / 2.0f, girlPos.y - girlOffsetY);
                playerHpBar.setPosition(girlPos.x - barWidth / 2.0f, girlPos.y - girlOffsetY);


            // --- エネミーのHPバー描画 ---
            for (const auto& enemy : enemies) {
                float enemyHpRatio = static_cast<float>(enemy.stats.hp) / 50.0f; // 初期HP=50想定
                sf::RectangleShape enemyHpBarBack(sf::Vector2f(barWidth, 6.0f));
                sf::RectangleShape enemyHpBarFill(sf::Vector2f(barWidth * enemyHpRatio, 6.0f));

                enemyHpBarBack.setFillColor(sf::Color(100, 100, 100));
                enemyHpBarFill.setFillColor(sf::Color::Green);

                sf::Vector2f ePos = enemy.sprite.getPosition();
                float enemyOffsetY = enemyFrameHeight * 0.2f;
                enemyHpBarBack.setPosition(ePos.x - barWidth / 2.0f, ePos.y - enemyOffsetY);
                enemyHpBarFill.setPosition(ePos.x - barWidth / 2.0f, ePos.y - enemyOffsetY);

                window.draw(enemyHpBarBack);
                window.draw(enemyHpBarFill);
            }

            }
            else {
                gameClock.restart();
                spawnClock.restart();
                enemySpawnClock.restart();
            }
            
            // --- 汚染ゲージ更新 ---
            float fillHeight = (pollution / pollutionMax) * 300.0f;
            pollutionBarFill.setSize(sf::Vector2f(30, fillHeight));
            pollutionBarFill.setPosition(20, 20 + 300 - fillHeight);
            
            // --- 描画 ---
            for (const auto& ts : trushSprites) {
                window.draw(ts);
            }
            for (const auto& enemy : enemies) {
                window.draw(enemy.sprite);
            }
            window.draw(girlSprite);
            window.draw(expBarBackground);
            window.draw(expBar);
            window.draw(pollutionBarBackground);
            window.draw(pollutionBarFill);
            window.draw(timerText);
            window.draw(playerHpBarBackground);
            window.draw(playerHpBar);
        }
        else if (currentState == GameState::Clear) {
            window.draw(clearText);
            window.draw(restartButton);
            window.draw(restartButtonText);

        }
        else if (currentState == GameState::GameOver) {
            window.draw(gameOverText);
            window.draw(restartButton);
            window.draw(restartButtonText);
        }

        // --- アップグレード画面描画 ---
        if (showUpgradeScreen) {

            window.draw(upgradeOverlay);
            bool isHovering = false;
            for (size_t i = 0; i < upgradeItems.size(); ++i) {
                window.draw(upgradeItems[i]);

                // --- ホバー時の説明文表示 ---
                sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                if (upgradeItems[i].getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
                    sf::Text descriptionText(upgradeDescriptions[i], font, 18);
                    descriptionText.setFillColor(sf::Color::White);
                    sf::FloatRect iconBounds = upgradeItems[i].getGlobalBounds();
                    sf::FloatRect textBounds = descriptionText.getLocalBounds();
                    descriptionText.setOrigin(textBounds.left + textBounds.width / 2.0f, textBounds.top + textBounds.height);
                    // アイコンの中央上に、十分な余白（例：20px）を空けて配置
                    descriptionText.setPosition(iconBounds.left + iconBounds.width / 2.0f, iconBounds.top - 20.0f);
                    window.draw(descriptionText);
                    // --- ★ここで upcomingChanges を更新する ---
                    upcomingChanges.clear();
                    if (i == 0) {
                            upcomingChanges.push_back({"moveSpeedUP", MOVE_SPEED_UP});
                            upcomingChanges.push_back({"revivalTimeUP", REVIVAL_TIME_UP});
                    } else if (i == 1) {
                            upcomingChanges.push_back({"cleanSpeedUP", CLEAN_SPEED_UP});
                            upcomingChanges.push_back({"attackPowerDOWN", ATTACK_POWER_DOWN});
                    } else if (i == 2) {
                            upcomingChanges.push_back({"maxHpUP", MAX_HP_UP});
                            upcomingChanges.push_back({"moveSpeedDOWN", MOVE_SPEED_DOWN});
                    } else if (i == 3) {
                            upcomingChanges.push_back({"attackPowerUP", ATTACK_POWER_UP});
                            upcomingChanges.push_back({"attackSpeedDOWN", ATTACK_SPEED_DOWN});
                    } else if (i == 4) {
                            upcomingChanges.push_back({"attackSpeedUP", ATTACK_SPEED_UP});
                            upcomingChanges.push_back({"cleanSpeedDOWN", CLEAN_SPEED_DOWN});
                    } else if (i == 5) {
                            upcomingChanges.push_back({"revivalTimeDOWN", REVIVAL_TIME_DOWN});
                            upcomingChanges.push_back({"maxHpDOWN", MAX_HP_DOWN});
                    }
                    isHovering = true;
                }
                if (!isHovering) {
                    upcomingChanges.clear();
                }
            }
            // --- 現在のステータス表示 ---
            std::ostringstream statsStream;
            statsStream << "[Current Stats]\n";
            // Speed
            statsStream << "Speed:        " << player.moveSpeed;
            if (isNearlyEqual(player.moveSpeed, MOVE_SPEED_MAX))
                statsStream << " (max)";
            else if (isNearlyEqual(player.moveSpeed, MOVE_SPEED_MIN))
                statsStream << " (min)";
            statsStream << "\n";

            // CleanSpeed
            statsStream << "CleanSpeed:   " << player.cleanSpeed << "s";
            if (isNearlyEqual(player.cleanSpeed, CLEAN_SPEED_MAX))
                statsStream << " (max)";
            else if (isNearlyEqual(player.cleanSpeed, CLEAN_SPEED_MIN))
                statsStream << " (min)";
            statsStream << "\n";

            // MaxHP
            statsStream << "MaxHP:        " << player.maxHp;
            if (player.maxHp >= MAX_HP_MAX)
                statsStream << " (max)";
            else if (player.maxHp <= MAX_HP_MIN)
                statsStream << " (min)";
            statsStream << "\n";

            // AttackPower
            statsStream << "AttackPower:  " << player.attackPower;
            if (isNearlyEqual(player.attackPower, ATTACK_POWER_MAX))
                statsStream << " (max)";
            else if (isNearlyEqual(player.attackPower, ATTACK_POWER_MIN))
                statsStream << " (min)";
            statsStream << "\n";

            // AttackSpeed
            statsStream << "AttackSpeed:  " << player.attackSpeed << "s";
            if (isNearlyEqual(player.attackSpeed, ATTACK_SPEED_MAX))
                statsStream << " (max)";
            else if (isNearlyEqual(player.attackSpeed, ATTACK_SPEED_MIN))
                statsStream << " (min)";
            statsStream << "\n";

            // RevivalTime
            statsStream << "RevivalTime:  " << player.revivalTime << "s";
            if (isNearlyEqual(player.revivalTime, REVIVAL_TIME_MAX))
                statsStream << " (max)";
            else if (isNearlyEqual(player.revivalTime, REVIVAL_TIME_MIN))
                statsStream << " (min)";
            statsStream << "\n";
            statsText.setString(statsStream.str());
            window.draw(statsText); // ← 最後にステータスを描画
            float baseX = statsText.getPosition().x + 230;
            float baseY = statsText.getPosition().y;
            float lineHeight = 23;
    
            for (size_t i = 0; i < upcomingChanges.size(); ++i) {
                sf::Text changeText;
                changeText.setFont(font);
                changeText.setCharacterSize(20);
                std::ostringstream val;
                val << std::showpos << upcomingChanges[i].second;
                changeText.setString(val.str());
                // std::cout << "Change[" << i << "]: " << changeText.getString().toAnsiString() << std::endl;
                // std::cout<<upcomingChanges[i].first<<std::endl;
                std::string key = upcomingChanges[i].first;
                if (statChangeColors.count(key)) {
                    changeText.setFillColor(statChangeColors[key]);
                } else {
                    changeText.setFillColor(sf::Color::White);
                }
                if(key == "moveSpeedUP" || key == "moveSpeedDOWN")
                {
                    changeText.setPosition(baseX, baseY + 1 * lineHeight);
                }else if(key == "cleanSpeedUP" || key == "cleanSpeedDOWN")
                {
                    changeText.setPosition(baseX, baseY + 2 * lineHeight);
                }else if(key == "maxHpUP" || key == "maxHpDOWN")
                {
                    changeText.setPosition(baseX, baseY + 3 * lineHeight);
                }else if(key == "attackPowerUP" || key == "attackPowerDOWN")
                {
                    changeText.setPosition(baseX, baseY + 4 * lineHeight);
                }else if(key == "attackSpeedUP" || key == "attackSpeedDOWN")
                {
                    changeText.setPosition(baseX, baseY + 5 * lineHeight);
                }else if(key == "revivalTimeUP" || key == "revivalTimeDOWN")
                {
                    changeText.setPosition(baseX, baseY + 6 * lineHeight);
                }
                window.draw(changeText);
            }
        }
        window.display();
    }
    return 0;
}
