#include "EnemyManager.h"
#include "Enemy.h"
#include "NormalEnemy.h"
#include "FastEnemy.h"
#include "SlowEnemy.h"
#include "GameContext.h"
#include <random>

EnemyManager::EnemyManager(){
	loadAllTexture();
	initialScheduleList();
}

void EnemyManager::reset(GameContext& context){
	getEnemySpawnScheduleList().clear();
	for(auto enemy : context.enemies)
	{
		delete enemy;
	}
	context.enemies.clear();
	this->initialScheduleList();
}

std::vector<EnemyManager::EnemySpawnSchedule>& EnemyManager::getEnemySpawnScheduleList(){
	return enemySpawnScheduleList;
}

bool EnemyManager::loadAllTexture(){
	if(!normal_texture_front.loadFromFile("./image/normal_enemy_left.png")) return false;
	if(!normal_texture_left.loadFromFile("./image/normal_enemy_left.png")) return false;
	if(!normal_texture_back.loadFromFile("./image/normal_enemy_right.png")) return false;
	if(!normal_texture_right.loadFromFile("./image/normal_enemy_right.png")) return false;
	if(!normal_texture_front_attack.loadFromFile("./image/normal_killedEnemy_left.png")) return false;
	if(!normal_texture_left_attack.loadFromFile("./image/normal_killedEnemy_left.png")) return false;
	if(!normal_texture_back_attack.loadFromFile("./image/normal_killedEnemy_right.png")) return false;
	if(!normal_texture_right_attack.loadFromFile("./image/normal_killedEnemy_right.png")) return false;
	if(!fast_texture_front.loadFromFile("./image/fast_enemy_left.png")) return false;
	if(!fast_texture_left.loadFromFile("./image/fast_enemy_left.png")) return false;
	if(!fast_texture_back.loadFromFile("./image/fast_enemy_right.png")) return false;
	if(!fast_texture_right.loadFromFile("./image/fast_enemy_right.png")) return false;
	if(!fast_texture_front_attack.loadFromFile("./image/fast_killedEnemy_left.png")) return false;
	if(!fast_texture_left_attack.loadFromFile("./image/fast_killedEnemy_left.png")) return false;
	if(!fast_texture_back_attack.loadFromFile("./image/fast_killedEnemy_right.png")) return false;
	if(!fast_texture_right_attack.loadFromFile("./image/fast_killedEnemy_right.png")) return false;
	if(!slow_texture_front.loadFromFile("./image/slow_enemy_left.png")) return false;
	if(!slow_texture_left.loadFromFile("./image/slow_enemy_left.png")) return false;
	if(!slow_texture_back.loadFromFile("./image/slow_enemy_right.png")) return false;
	if(!slow_texture_right.loadFromFile("./image/slow_enemy_right.png")) return false;
	if(!slow_texture_front_attack.loadFromFile("./image/slow_killedEnemy_left.png")) return false;
	if(!slow_texture_left_attack.loadFromFile("./image/slow_killedEnemy_left.png")) return false;
	if(!slow_texture_back_attack.loadFromFile("./image/slow_killedEnemy_right.png")) return false;
	if(!slow_texture_right_attack.loadFromFile("./image/slow_killedEnemy_right.png")) return false;
	// if(!boss_texture_front.loadFromFile("./image/enemy_left.png")) return false;
	// if(!boss_texture_left.loadFromFile("./image/enemy_left.png")) return false;
	// if(!boss_texture_back.loadFromFile("./image/enemy_right.png")) return false;
	// if(!boss_texture_right.loadFromFile("./image/enemy_right.png")) return false;
	// if(!boss_texture_front_attack.loadFromFile("./image/killedEnemy_left.png")) return false;
	// if(!boss_texture_left_attack.loadFromFile("./image/killedEnemy_left.png")) return false;
	// if(!boss_texture_back_attack.loadFromFile("./image/killedEnemy_right.png")) return false;
	// if(!boss_texture_right_attack.loadFromFile("./image/killedEnemy_right.png")) return false;
	return true;
};

void EnemyManager::initialScheduleList() {
    enemySpawnScheduleList.clear();

    float currentTime = 2.0f;
    const float totalTime = 300.0f;

    EnemyType types[] = {EnemyType::Normal, EnemyType::Slow, EnemyType::Fast};
    Enemy::Direction dirs[] = {
        Enemy::Direction::up,
        Enemy::Direction::down,
        Enemy::Direction::left,
        Enemy::Direction::right
    };
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_real_distribution<float> intervalDist(2.0f, 8.0f);
    std::uniform_int_distribution<int> dirDist(0, 3);

    int rushIndex = 0; // ラッシュ回数カウント（0始まり）

    while (currentTime <= totalTime) {
        bool inRushZone = false;
        int currentRushStart = 0;

        // 現在がラッシュゾーンかどうか判定
        if (static_cast<int>(currentTime) % 30 < 10) {
            inRushZone = true;
            currentRushStart = static_cast<int>(currentTime / 30) * 30;
            rushIndex = currentRushStart / 30; // 0〜10くらい
        }

        if (inRushZone) {
            // ラッシュゾーン：0.5秒ごとに出現
            float rushEnd = currentRushStart + 10.0f;
            while (currentTime < rushEnd) {
                // Fast の出現率を段階的に上げる
                float fastRate = 0.1f + 0.08f * rushIndex; // 初回10%、以降+8%ずつ（最大90%）

                float roll = std::generate_canonical<float, 10>(gen);
                EnemyType type;
                if (roll < fastRate) {
                    type = EnemyType::Fast;
                } else if (roll < fastRate + 0.3f) {
                    type = EnemyType::Slow;
                } else {
                    type = EnemyType::Normal;
                }

                Enemy::Direction dir = dirs[dirDist(gen)];
                enemySpawnScheduleList.push_back({type, dir, currentTime});

                currentTime += 0.5f;
            }
        } else {
            // 通常ゾーン
            EnemyType type = types[std::uniform_int_distribution<int>(0, 2)(gen)];
            Enemy::Direction dir = dirs[dirDist(gen)];
            enemySpawnScheduleList.push_back({type, dir, currentTime});
            currentTime += intervalDist(gen);
        }
    }
}

Enemy* EnemyManager::spawnEnemy(EnemyType type, Enemy::Direction dir){
	switch(type){
		case EnemyType::Normal:
			return new NormalEnemy(dir);
		case EnemyType::Fast:
			return new FastEnemy(dir);
		case EnemyType::Slow:
			return new SlowEnemy(dir);
		// case EnemyType::Boss:
		// 	return new Boss(dir);
		default:
			return nullptr;
	}
};

std::vector<Enemy*>::iterator EnemyManager::deleteEnemies(Enemy* enemy, std::vector<Enemy*>& chars){
	auto it = std::find(chars.begin(), chars.end(), enemy);
	if(it != chars.end()){
		delete *it;
		return chars.erase(it);
	}
	return chars.end();
}

void EnemyManager::generateEnemies(GameContext& context, float& gameTime){
        sf::Time delta = context.progressClock.restart();
        while(!context.enemyManager.getEnemySpawnScheduleList().empty()){
            if(context.spManager.getCurrentState() == SPManager::SPState::Idle)
            {
                gameTime += delta.asSeconds();
                const auto& schedule = context.enemyManager.getEnemySpawnScheduleList().front();
                // 時間がまだ来てない場合、残りはすべて後
                if (schedule.spawnSeconds > gameTime){
                    break;
                }
                // 時間が来ているので生成
                std::cout <<"スポーンした"<< std::endl;
                context.spawned = context.enemyManager.spawnEnemy(schedule.type, schedule.direction);
                if(context.spawned){
                    context.spawned->setEnemyTexture(*context.spawned, context.enemyManager);
                    // spawned->setScale(0.1);
                    context.spawned->setEnemyPos(context.spawned->getMoveSpeed(), context.window);
                    context.enemies.push_back(context.spawned);
                }
                // 生成済みなので削除
                context.enemyManager.getEnemySpawnScheduleList().erase(
                    context.enemyManager.getEnemySpawnScheduleList().begin()
                );
            }else{
                break;
            }
        }
}

void EnemyManager::updateEnemies(GameContext& context){
	auto it = context.enemies.begin();
	while(it != context.enemies.end()){
		Enemy* enemy = *it;
		if(context.spManager.getCurrentState() == SPManager::SPState::Idle){
			enemy->moveEnemy(enemy->getMoveSpeed());//エネミーを動かす処理
		}
		enemy->setEnemyTexture(*enemy, context.enemyManager);//エネミーテクスチャの変更
		bool isFade = enemy->fadeEnemySprite(enemy->getFadeSpeed(), context.spManager);
		if(isFade){
			std::cout << "fadeした" << std::endl;
			it = context.enemyManager.deleteEnemies(enemy, context.enemies);
			continue; // erase後はincrement禁止
		}
		if(enemy != nullptr){
			context.player.setTarget(enemy);
			context.player.rockOnOutLine(context.outline);
		}
		if(context.player.enemyIsHitted(*enemy)){
			context.player.defeated(context, *enemy);
		}
		for(auto& cir : context.circles){
			cir->checkContact(*enemy);//enemyがどの円に触れているか
		}
		++it;
	}
}
