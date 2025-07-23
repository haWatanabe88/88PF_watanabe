window.addEventListener("load",init);
//目のサイズ
let whiteEye_radius = 80;
let whiteEye_diameter = whiteEye_radius * 2;
let blackEye_radius = whiteEye_radius * 0.68;

//目と目の間
let eye_dir = whiteEye_radius * 0.2;

function init(){
    console.log("起動確認");
    //ステージの作成
    let  stage = new createjs.Stage("mycanvas");
///////////////////////眼球生成//////////////////////////////////////////////
//眼球_左
    //白目
    let  L_eye_W = new createjs.Shape();
    L_eye_W.graphics.beginStroke("black").drawCircle(0,0,whiteEye_radius);
    L_eye_W.x = window.innerWidth / 2 - whiteEye_radius - eye_dir;
    L_eye_W.y = window.innerHeight / 2;
    // stage.addChild(L_eye_W);
    //黒目
    let  L_eye_B = new createjs.Shape();
    L_eye_B.graphics.beginFill("black").drawCircle(0,0,blackEye_radius);
    L_eye_B.x = window.innerWidth / 2 - whiteEye_radius - eye_dir;
    L_eye_B.y = window.innerHeight / 2;
    // stage.addChild(L_eye_B);
    //まぶた
    let  L_eyelid = new createjs.Shape();
    L_eyelid.graphics.beginFill("white").drawCircle(0,0,whiteEye_radius);
    L_eyelid.x = window.innerWidth / 2 - whiteEye_radius - eye_dir;
    // stage.addChild(L_eyelid);

    //瞬きが自然に見えるように、appendする順番を調整
    stage.addChild(L_eye_B);
    stage.addChild(L_eyelid);
    stage.addChild(L_eye_W);

    ///////////////////////////////////////スマイル
    let smileEye_L = new createjs.Shape();
    let smileEye_R = new createjs.Shape();
    let adj_smile = 100;
    ///*左目用スマイルeye
    smileEye_L.graphics.beginStroke("black")
                    .setStrokeStyle(7,"round")
                                                                        //微調整
                    .moveTo(0,0).quadraticCurveTo(40 + whiteEye_radius - 40,-90,whiteEye_diameter,0);
    smileEye_L.x = window.innerWidth / 2 - whiteEye_diameter - eye_dir;
    smileEye_L.y = window.innerHeight / 2;

    //右目用スマイルeye
    smileEye_R.graphics.beginStroke("black")
                    .setStrokeStyle(7,"round")
                                                                        //微調整
                    .moveTo(0,0).quadraticCurveTo(40 + whiteEye_radius - 40,-90,whiteEye_diameter,0);
    smileEye_R.x = window.innerWidth / 2 + eye_dir;
    smileEye_R.y = window.innerHeight / 2;
    //スマイルeys非表示
    smileEye_L.visible = false;
    smileEye_R.visible = false;

    //*/
    //Enter（キーコード13）で瞬きするイベント => flag を 10 ~> 0へ
    window.addEventListener("keydown",changeEye);
    function changeEye(event){
        var keyCode = event.keyCode;
        //console.log(keyCode);
        if(keyCode == 18){  //スマイルeye optionキー
            smileEye_L.visible = true;
            smileEye_R.visible = true;
            L_eye_B.visible = false;
            R_eye_B.visible = false;
            console.log("Tab");
        }else if(keyCode == 17){ //スマイルeye_close ctl
            smileEye_L.visible = false;
            smileEye_R.visible = false;
            L_eye_B.visible = true;
            R_eye_B.visible = true;
        }
    }
    stage.addChild(smileEye_L);
    stage.addChild(smileEye_R);
///////////////////////////////////////スマイル 終了
//眼球_右   
    //白目
    let  R_eye_W = new createjs.Shape();
    R_eye_W.graphics.beginStroke("black").drawCircle(0,0,whiteEye_radius);
    R_eye_W.x = window.innerWidth / 2 + whiteEye_radius + eye_dir;
    R_eye_W.y = window.innerHeight / 2;
    // stage.addChild(R_eye_W);
    //黒目
    let  R_eye_B = new createjs.Shape();
    R_eye_B.graphics.beginFill("black").drawCircle(0,0,blackEye_radius);
    R_eye_B.x = window.innerWidth / 2 + whiteEye_radius + eye_dir;
    R_eye_B.y = window.innerHeight / 2;
    // stage.addChild(R_eye_B);
    //まぶた
    let  R_eyelid = new createjs.Shape();
    R_eyelid.graphics.beginFill("white").drawCircle(0,0,whiteEye_radius);
    R_eyelid.x = window.innerWidth / 2 + whiteEye_radius + eye_dir;
    // stage.addChild(R_eyelid);

    //瞬きが自然に見えるように、appendする順番を調整
    stage.addChild(R_eye_B);
    stage.addChild(R_eyelid);
    stage.addChild(R_eye_W);

/////////////////////////////////////////////////////////////////////

    //まぶたの下がる速度の変数
    let count = 0;
    //0 , 1 意外ならなんでもいい
    let flag = 10;

    //Enter（キーコード13）で瞬きするイベント => flag を 10 ~> 0へ
    window.addEventListener("keydown",keyDown);
    function keyDown(event){
        var keyCode = event.keyCode;
        //console.log(keyCode);
        if(keyCode == 13 || keyCode == 16){  //Enter => ver3以降 Shiftも追加
            console.log("Enter");
            //まぶたが降りるflagに変更
            flag = 0;
        }
    }

    //黒目
    let eye_B_posx = 0;
    let eye_B_posy = 0;
    let eyelidSpeed = 17;
///////////////////////////////////////
//      スリープモードに関するもの
///////////////////////////////////////

    let sleepEyelid_L = new createjs.Shape();
    let sleepEyelid_R = new createjs.Shape();
    let sleepB_E = new createjs.Shape();
    let underLinePos_adj = 56;

    let ZzzPosX = window.innerWidth / 2;
    let ZzzPosY = window.innerHeight / 2;
    let Z1 = new createjs.Text("Z","36px sans-selif","blue");
    let Z2 = new createjs.Text("z","36px sans-selif","blue");
    let Z3 = new createjs.Text("z","30px sans-selif","blue");

    let sleepCountDown = 0;
    let sleepFlag = false;

    window.addEventListener("keydown",sleepyMode);
    function sleepyMode(event){
        var keyCode = event.keyCode;
        if(keyCode == 32){ //spaceキー
            flag = 2;
            sleepCountDown += 1;
            console.log(sleepCountDown);

            eyelidSpeed = 1.5;
            posSpeed = 0.001;

            //平常時のまぶたを見えなくしている => もうちょい考えるよちがある気がする。。。
            L_eyelid.visible = false;
            R_eyelid.visible = false;
            stage.removeChild(L_eye_W);
            stage.removeChild(R_eye_W);

            //上から降りてくるビッグまぶた
            sleepB_E.graphics.beginFill("white").arc(0,0,whiteEye_diameter + eye_dir + 10,Math.PI,Math.PI * 2);
            sleepB_E.x = window.innerWidth / 2;
            sleepB_E.y = window.innerHeight / 2;

            stage.addChild(sleepB_E);
            stage.addChild(L_eye_W);
            stage.addChild(R_eye_W);

            sleepEyelid_L.graphics.beginFill("white")
                                .arc(0,0,whiteEye_radius - 1.5,Math.PI,Math.PI * 2).closePath();
            sleepEyelid_R.graphics.beginFill("white")
                                .arc(0,0,whiteEye_radius - 1.5,Math.PI,Math.PI * 2);
            //左目
            sleepEyelid_L.x = window.innerWidth / 2 - whiteEye_radius - eye_dir;
            sleepEyelid_L.y = window.innerHeight / 2;
            //右目
            sleepEyelid_R.x = window.innerWidth / 2 + whiteEye_radius + eye_dir;
            sleepEyelid_R.y = window.innerHeight / 2;

            ////////
            // 下瞼
            ////////
            let underLine_L = new createjs.Shape();
            let underLine_R = new createjs.Shape();

            //左目
            underLine_L.graphics.beginFill("white")
                                .beginStroke("black")
                                .arc(L_eye_W.x ,     //円弧の中心のx座標
                                    L_eye_W.y,     //円弧の中心のy座標
                                    whiteEye_radius,       //半径
                                    Math.PI/6,             //startAngle
                                    Math.PI - Math.PI/6)   //endAngle
                                .closePath();               //始点と終点を結ぶ
            //右目
            underLine_R.graphics.beginFill("white")
                                .beginStroke("black")
                                .arc(R_eye_W.x ,     //円弧の中心のx座標
                                    R_eye_W.y,     //円弧の中心のy座標
                                    whiteEye_radius,       //半径
                                    Math.PI/6,             //startAngle
                                    Math.PI - Math.PI/6)   //endAngle
                                .closePath();               //始点と終点を結ぶ

            stage.addChild(underLine_L);
            stage.addChild(underLine_R);
            stage.addChild(sleepEyelid_L);
            stage.addChild(sleepEyelid_R);
        
            //Zzzアニメーション関連
            stage.addChild(Z1);
            stage.addChild(Z2);
            stage.addChild(Z3);
            Z1.visible = false;
            Z2.visible = false;
            Z3.visible = false;  
    }
}
///////////////////////////////////////
//         充血モードに関するもの
///////////////////////////////////////
    let bldFlag = false;
    let tearFlag = false;
    let bld_alpha = 0;
    let tear_alpha = 0;

    let tearRadius_w = 90;
    let tearRadius_h = 55;

    let tear_L = new createjs.Shape();
    let tear_R = new createjs.Shape();

    // let tDF1 = true;
    // let tDF2 = false;
    // let tDF3 = false;
    // let tDF4 = false;


    // let resetFlag = true;

    //涙っぽい表示がされる位置
    //stageに追加するのは一回だけ
    stage.addChild(tear_L);
    stage.addChild(tear_R);

    window.addEventListener("keydown",bldMode);
    function bldMode(event){
       var keyCode = event.keyCode;
       if(keyCode == 37){ //←キー
            bldFlag = true;
        }
    }

/////ver8.0 以降追加
///*    
    // let tearDropFlag = false;
    let tearDropCon_R = new createjs.Container();
    let tearDropCon_L = new createjs.Container();

    //tearDropCon_R.visible = tearDropCon_L.visible = false;
    //shape
    let tearDrop_R = new createjs.Shape();
    let tearDrop_L = new createjs.Shape();

    let spx_R = R_eye_W.x - whiteEye_radius - 5 + 150;   //tearDropの開始点
    let spx_L = L_eye_W.x + whiteEye_radius + 5 - 150;
    let spy = R_eye_W.y - blackEye_radius / 2 - 5;

    //tearDropの動く速度
    let tweenScond = 320;


    // cryEye_R.x = R_eye_W.x - whiteEye_radius - 5;
    // cryEye_R.y = R_eye_W.y - blackEye_radius / 2;

    //左目
    // cryEye_L.x = L_eye_W.x + whiteEye_radius + 5;
    // cryEye_L.y = L_eye_W.y - blackEye_radius / 2;

    //tear
    //形作成
    tearDrop_R.graphics.beginFill("lightskyblue").drawCircle(0,0,40);
    tearDropCon_R.x = spx_R;
    tearDropCon_R.y = spy;

    tearDrop_L.graphics.beginFill("lightskyblue").drawCircle(0,0,40);
    tearDropCon_L.x = spx_L;
    tearDropCon_L.y = spy;

    //チェック用
    // stage.addChild(tearDropCon_R);
    // stage.addChild(tearDropCon_L);
    // tearDropCon_R.addChild(tearDrop_R);
    // tearDropCon_L.addChild(tearDrop_L);

    createjs.Tween.get(tearDropCon_R ,{loop:true})
                    .to({x:spx_R + 190 ,y:spy + 125,scale:0.8},tweenScond)
                    .to({x:spx_R + 260,y:spy + 170,scale:0.6},tweenScond)
                    .to({x:spx_R + 330,y:spy + 245,scale:0.4},tweenScond);
    createjs.Tween.get(tearDropCon_L,{loop:true})
                    .to({x:spx_L - 190 ,y: spy + 125,scale:0.8},tweenScond)
                    .to({x:spx_L - 260,y:spy + 170,scale:0.6},tweenScond)
                    .to({x:spx_L - 330,y:spy + 245,scale:0.4},tweenScond); 
//*/

/////ver8.0 以降追加部分 終了


    let posSpeed = 0.002;
///////////////////////////////////////
//             描画処理部分
///////////////////////////////////////
    createjs.Ticker.addEventListener("tick",handle);
    function handle(){
        console.log("flag:" + flag);
        //まばたき関連
            //平常時：まぶた降りる
            if(flag == 0){
                //console.log("描画");
                //左目のまぶた
                L_eyelid.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                //右目のまぶた
                R_eyelid.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                //瞬きの速度調整
                count += eyelidSpeed;
                //まぶたが下までおり切ったら・・・
                if(count > whiteEye_diameter){
                    flag = 1;
                }
            }
            if(flag == 1){
                // console.log("描画");
                //左目のまぶた
                L_eyelid.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                //右目のまぶた
                R_eyelid.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                //瞬きの速度調整
                count -= eyelidSpeed;
                //まぶたが上まで上がり切ったら・・・
                if(count < 0){
                    flag = 10;
                }
            }
            /////////////////////
            //sleepモード関連のflag
            /////////////////////
            if(flag == 2){
                sleepB_E.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                count += eyelidSpeed;            

                if(count > whiteEye_diameter + underLinePos_adj){
                    if(sleepCountDown > 2){
                        sleepFlag = true;
                    }else{
                        flag = 3;
                    }
                }
            }
            if(flag == 3){
                sleepB_E.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                count -= eyelidSpeed;
                if(count < 0){
                    flag = 10;
                }
            }
            if(sleepFlag == true){
                console.log("sleepMode起動");
                Z1.visible = true;
                Z2.visible = true;
                Z3.visible = true;
                //ちょっとゴリ押し感でてるので、、、後々うまい方法が見つかり次第改良・・・
                L_eye_B.visible = false;
                R_eye_B.visible = false;
                sleepB_E.y = window.innerHeight / 2 - (whiteEye_diameter - count);
                //Zzzアニメーション関連
                Z1.x = ZzzPosX + 40;
                Z1.y = ZzzPosY - 40;
                Z2.x = ZzzPosX + 19;
                Z2.y = ZzzPosY - 19;
                Z3.x = ZzzPosX;
                Z3.y = ZzzPosY;

                ZzzPosX += 1;
                ZzzPosY -= 1;
                if(ZzzPosX > window.innerWidth / 2 + 100){
                    ZzzPosX = window.innerWidth / 2;
                    ZzzPosY = window.innerHeight / 2;
                }
            }
            /////////////////////
            //bldモード関連のflag
            /////////////////////
            if(bldFlag == true){
                //黒目が動かないようにした ＝＞ ちょっと動くけど。。。 もうちょいうまい方法ありそう・・・
                L_eye_B.x = L_eye_W.x;
                L_eye_B.y = L_eye_W.y; 
                R_eye_B.x = R_eye_W.x; 
                R_eye_B.y = R_eye_W.y; 

                let bldContainer_L = new createjs.Container();
                let bldContainer_R = new createjs.Container();

                stage.addChild(bldContainer_L);
                stage.addChild(bldContainer_R);

                let radius = whiteEye_radius; //80 一番上にある
                //bldの書き出し s~   書き終わり e~
                let sx;
                let sy;
                let ex;
                let ey;

                //bldの長さ調整
                let bld_adj = 20;
                let bld_weight = 3.5;
                // let bld_alpha = 0;

                bldContainer_L.alpha = bldContainer_R.alpha = bld_alpha;

                //充血の線 を 12本設定
                //左目
                for(let deg=0;deg < 360; deg+=30){
                    let bld  = new createjs.Shape();
                    sx = radius * Math.sin(deg * Math.PI / 180);
                    sy = radius * Math.cos(deg * Math.PI / 180);
                    ex = (radius-bld_adj) * Math.sin(deg * Math.PI / 180);
                    ey = (radius-bld_adj) * Math.cos(deg * Math.PI / 180);
                    bld.graphics.beginStroke("DarkRed").setStrokeStyle(bld_weight)
                                                    .moveTo(sx,sy)
                                                    .lineTo(ex,ey);
                    bldContainer_L.addChild(bld);
                }
                //右目
                for(let deg=0;deg < 360; deg+=30){
                    let bld  = new createjs.Shape();
                    sx = radius * Math.sin(deg * Math.PI / 180);
                    sy = radius * Math.cos(deg * Math.PI / 180);
                    ex = (radius-bld_adj) * Math.sin(deg * Math.PI / 180);
                    ey = (radius-bld_adj) * Math.cos(deg * Math.PI / 180);
                    bld.graphics.beginStroke("DarkRed").setStrokeStyle(bld_weight)
                                                    .moveTo(sx,sy)
                                                    .lineTo(ex,ey);
                    bldContainer_R.addChild(bld);
                }
                bldContainer_L.x = L_eye_W.x;
                bldContainer_L.y = L_eye_W.y;
                bldContainer_R.x = R_eye_W.x;
                bldContainer_R.y = R_eye_W.y;

                if(bld_alpha < 1){
                    //大体15秒ぐらいで"tearFlag起動"する
                    bld_alpha  += 0.0025 * 2;
                    console.log(bld_alpha)
                }else{
                    console.log("tearFlag起動");
                    tearFlag = true;
                    console.log(tearFlag);
                }
                //溜まっていく涙 の部分
                if(tearFlag == true){
                    if(tear_alpha < 1){
                        // let tear_L = new createjs.Shape();
                        // let tear_R = new createjs.Shape();
                        // stage.addChild(tear_L);
                        // stage.addChild(tear_R);

                        //一番最初は見えていない状態
                        tear_L.alpha = tear_R.alpha = tear_alpha;
                        tear_alpha += 0.0025 * 2 * 2;
                        tearRadius_w += 0.2;
                        tearRadius_h += 0.2;
                        //console.log(tear_alpha);

                        // let tearRadius_w = 90;
                        // let tearRadius_h = 55;
                        //基準点をオブジェクトの中心座標に変更
                        tear_L.regX = tear_R.regX = tearRadius_w / 2;
                        tear_L.regY = tear_R.regY = tearRadius_h / 2;

                        tear_L.graphics.beginFill("lightskyblue").drawEllipse(0,0,tearRadius_w,tearRadius_h);
                        tear_R.graphics.beginFill("lightskyblue").drawEllipse(0,0,tearRadius_w,tearRadius_h);

                        tear_L.x = L_eye_W.x - whiteEye_radius / 2;
                        tear_R.x = R_eye_W.x + whiteEye_radius / 2;
                        tear_L.y = tear_R.y = L_eye_W.y + whiteEye_radius;

                    }else{
                        // if(resetFlag == true){
                        //     console.log("resetFlag " + resetFlag);
                            //全除去
                            stage.removeAllChildren();
                            // resetFlag = false;
                            tearDropFlag = true;
                            console.log(tearDropFlag);
                        // }
                        //必要なものを追加
                        stage.addChild(L_eye_W);
                        stage.addChild(R_eye_W);

                        //再追加することで回避・・・ver8.0以降
                        stage.addChild(tearDropCon_R);
                        stage.addChild(tearDropCon_L);
                        tearDropCon_R.addChild(tearDrop_R);
                        tearDropCon_L.addChild(tearDrop_L);

                        //泣いている目の部分の右左上下
                        let up_R = new createjs.Shape();
                        let down_R = new createjs.Shape();
                        let cryEye_R = new createjs.Container();

                        let up_L = new createjs.Shape();
                        let down_L = new createjs.Shape();
                        let cryEye_L = new createjs.Container();

                        stage.addChild(cryEye_R);
                        stage.addChild(cryEye_L);

                        //quadraticCurveToはものすごく微調整している
                        //右目
                        up_R.graphics.beginStroke("black")
                                        .setStrokeStyle(4,"round")
                                        .moveTo(10,10).quadraticCurveTo(25,-10,170,-20);
                        down_R.graphics.beginStroke("black")
                                        .setStrokeStyle(4,"round")
                                        .moveTo(10,10).quadraticCurveTo(90,-15,170,-10);
                        down_R.rotation = 25;
                        down_R.x += 6;
                        down_R.y += -3;
                        // cryEye_R.x = 600;
                        cryEye_R.x = R_eye_W.x - whiteEye_radius - 5;
                        cryEye_R.y = R_eye_W.y - blackEye_radius / 2;

                        //左目
                        up_L.graphics.beginStroke("black")
                                        .setStrokeStyle(4,"round")
                                        .moveTo(10,10).quadraticCurveTo(25,-10,170,-20);
                        down_L.graphics.beginStroke("black")
                                        .setStrokeStyle(4,"round")
                                        .moveTo(10,10).quadraticCurveTo(90,-15,170,-10);
                        down_L.rotation = 25;
                        down_L.x += 6;
                        down_L.y += -3;
                        // cryEye_L.x = 300;
                        // cryEye_L.y = 100;
                        cryEye_L.x = L_eye_W.x + whiteEye_radius + 5;
                        cryEye_L.y = L_eye_W.y - blackEye_radius / 2;
                        //設定は右目と同じで、コンテナを反転させている
                        cryEye_L.scaleX = -1.0;
                        
                        cryEye_L.rotation = 6;
                        cryEye_R.rotation = -6;

                        cryEye_R.addChild(up_R);
                        cryEye_R.addChild(down_R);
                        cryEye_L.addChild(up_L);
                        cryEye_L.addChild(down_L);
                        }
                    }
                }

        ////ver3以降
        //黒目の移動速度調整係数
        // let posSpeed = 0.002;
        //黒目を動かす
        eye_B_posx = (stage.mouseX - window.innerWidth / 2) * posSpeed;
        eye_B_posy = (stage.mouseY - window.innerHeight / 2) * posSpeed;

        //白目と黒目の端っこ処理の際の調整分
        let adj = 9;

        ////目がはみ出ないように    
        //左目_黒
        //x座標関連
        if(L_eye_B.x + blackEye_radius > L_eye_W.x + whiteEye_radius - adj){
            L_eye_B.x  = L_eye_W.x + whiteEye_radius - blackEye_radius - adj;         
        }else if(L_eye_B.x - blackEye_radius < L_eye_W.x - whiteEye_radius + adj){
            L_eye_B.x  = L_eye_W.x - whiteEye_radius + adj + blackEye_radius;
        }else{
            L_eye_B.x += eye_B_posx;
        }
        //y座標関連
        if(L_eye_B.y + blackEye_radius > L_eye_W.y + whiteEye_radius - adj){
            L_eye_B.y  = L_eye_W.y + whiteEye_radius - blackEye_radius - adj;
        }else if(L_eye_B.y - blackEye_radius < L_eye_W.y - whiteEye_radius + adj){
            L_eye_B.y  = L_eye_W.y - whiteEye_radius + adj + blackEye_radius;
        }
        else{
            L_eye_B.y += eye_B_posy;
        }
        //右目_黒
        //x座標関連
        if(R_eye_B.x + blackEye_radius > R_eye_W.x + whiteEye_radius - adj){
            R_eye_B.x  = R_eye_W.x + whiteEye_radius - blackEye_radius - adj;         
        }else if(R_eye_B.x - blackEye_radius < R_eye_W.x - whiteEye_radius + adj){
            R_eye_B.x  = R_eye_W.x - whiteEye_radius + adj + blackEye_radius;
        }else{
            R_eye_B.x += eye_B_posx;
        }
        //y座標関連
        if(R_eye_B.y + blackEye_radius > R_eye_W.y + whiteEye_radius - adj){
            R_eye_B.y  = R_eye_W.y + whiteEye_radius - blackEye_radius - adj;
        }else if(R_eye_B.y - blackEye_radius < R_eye_W.y - whiteEye_radius + adj){
            R_eye_B.y  = R_eye_W.y - whiteEye_radius + adj + blackEye_radius;
        }
        else{
            R_eye_B.y += eye_B_posy;
        }

        //フレームレートの操作＝＞60fpsへ
        createjs.Ticker.timingMode = createjs.Ticker.RAF;
        stage.update();
    }
}