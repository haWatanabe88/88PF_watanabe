#include "GameContext.h"
#include "GameOverContext.h"
#include "GameTitleContext.h"

int main() {
    GameContext context;
    GameOverContext context_go(context.window, context);
    GameTitleContext context_ti(context.window, context);
    GameContext::SceneState previousScene = context.currentScene;

    float gameTime = 0;
    context.progressClock.restart();//ゲームプレイシーンの開始と同時にリセットする要素

    //画面がオープンしたあと
    while (context.window.isOpen()){
        sf::Event event;
        while (context.window.pollEvent(event)) {
		if (event.type == sf::Event::Closed)
			context.window.close();
        }
        // シーンが切り替わったか確認
        if (context.currentScene != previousScene) {
            if (context.currentScene == GameContext::SceneState::Playing) {
                context.progressClock.restart(); // Playingに入った瞬間に時計リセット
                gameTime = 0;
            }
            previousScene = context.currentScene;
        }
        switch (context.currentScene)
        {
            case GameContext::SceneState::Title:
                context.reset(context);
                context_go.reset();
                context_ti.draw(context.window);
                context_ti.update(context.window);
                break;
            case GameContext::SceneState::Playing:
                //windowの設定(D)
                context.settingWindow();
                //タイマー更新
                context.updateTimer();
                //敵の出現処理（U）
                context.enemyManager.generateEnemies(context, gameTime);
                //敵のフレーム処理（U）
                context.enemyManager.updateEnemies(context);
                //playerの操作処理（H）
                context.playerManager.handlePlayerInput(context);
                //必殺技のリリースチェック(U)
                context.spManager.checkSPRelease(context);
                //playerのテクスチャ切り替え(U)
                context.player.setTexture(context.playerManager, context.spManager);//テクスチャの切り替え担当
                //描画処理(D)
                context.draw();
                break;
            case GameContext::SceneState::GameOver:
                gameTime = 0;
                context_go.draw(context.window, context.scoreManager.getScore());
                context_go.update(context.window);
                break;
            default:
                break;
        }
    }
    return 0;
}