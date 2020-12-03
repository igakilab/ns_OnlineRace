# OnlineRace

## 概要

OnlineRaceは，PCはもちろん，スマートフォンやタブレットでもブラウザ上で遊ぶことができるオンラインマルチプレイ対応のタイムアタック式レースゲームです．セットアップ・運用方法のマニュアルを以下に記述しています．

## 準備

### Unityのインストールおよびモジュールの追加

まず，[UnityHub](https://unity3d.com/jp/get-unity/download)をインストールしてください．  
次に[こちら](https://unity3d.com/unity/qa/lts-releases)から`Unity(2019.4.11f1)`をダウンロードしてインストールしてください．インストールする際，追加するモジュールを聞かれるので`2019.4.11f1`と`WebGL Build Support`にチェックを入れてインストールしてください．  
![モジュール追加](https://user-images.githubusercontent.com/56796881/100954445-eb3cf680-3557-11eb-8927-f8a0e4fa550e.png)  
両方のインストールが完了したらUnityHubを起動し，`インストール`の`リストに追加`からインストールしたUnity(2019.4.11f1)のフォルダ内にある`Unity.exe`を選択して追加してください．  
![Unity追加](https://user-images.githubusercontent.com/56796881/100958718-2fcc9000-3560-11eb-82b9-51a67b2a7d21.png)  

### Photonアカウントの作成およびPUNアプリケーションの作成

[こちら](https://www.photonengine.com/)からPhotonアカウントの作成を行ってください．  
サインイン後，[Dashboard](https://dashboard.photonengine.com/)に移動し`CREATE A NEW APP`をクリックします．  
`CREATE A NEW APP`のページで以下のように入力し，`CREATE`をクリックしてください．  

```
Photon Type : Photon PUN  
Name : OnlineRace  
Description : (アプリケーションの説明，省略可)  
Url : (空欄)  
```

![PUNアプリ作成](https://user-images.githubusercontent.com/56796881/100954448-ebd58d00-3557-11eb-9b74-d7d49cac0f35.png)  
作成したPUNアプリがダッシュボードに表示されていれば完了です．  
![ダッシュボード](https://user-images.githubusercontent.com/56796881/100954451-ec6e2380-3557-11eb-8673-ec3681b923b7.png)  

### OnlineRaceのダウンロード

[こちら](https://github.com/igakilab/ns_OnlineRace)からOnlineRaceをダウンロードして解凍してください．  

### Unityの起動

UnityHubの`プロジェクト`の`リストに追加`からダウンロードして解凍した`ns_OnlineRace-main`フォルダを選択して追加してください．  
![プロジェクト追加](https://user-images.githubusercontent.com/56796881/100958716-2f33f980-3560-11eb-9587-0d2d52edbd28.png)  
追加後，プロジェクトをクリックしUnityを起動します．起動後に`Unity Editor Update Check`ウィンドウが開いた場合は`Skip new version`ボタンをクリックしてください．  
![プロジェクトリスト](https://user-images.githubusercontent.com/56796881/100954455-ed06ba00-3557-11eb-9b8e-7bd576dd29d2.png)  

### PUNのセットアップ

Unityのメニューで，[Window]->[Photon Unity Networking]->[PUN Wizard]から`PUN Wizard`ウィンドウを開きます．  
![PUN Wizard](https://user-images.githubusercontent.com/56796881/100954457-ed06ba00-3557-11eb-81a8-e11c8a4d0938.png)  
次に`Setup Project`ボタンをクリックして，`AppId or Email`の入力欄にPhotonのDashboardにある作成したアプリケーションの`AppID`を貼り付け，`Setup Project`ボタンをクリックします．  
![Setup Project](https://user-images.githubusercontent.com/56796881/100970970-66fb6b00-3579-11eb-90d9-a6250b0a0914.png)  
`AppID`は以下の画像の場所にあります．  
![AppID](https://user-images.githubusercontent.com/56796881/100954458-ed9f5080-3557-11eb-9fc0-5fb09f0ec482.png)  
セットアップが完了すると，PhotonServerSettingsのInspectorタブが開くので`Fixed Region`の入力欄に`jp`と入力します．  
開いていない場合は，[Project]->[Photon]->[PhotonUnityNetworking]->[Resources]にあるPhotonServerSettingsをクリックすることでInspectorタブを開けられます．  
![Inspectorタブ](https://user-images.githubusercontent.com/56796881/100954463-ee37e700-3557-11eb-9aa0-bce8b25b285c.png)  

### WebGLでビルド

[File]->[Build Settings]で`Build Settings`ウィンドウを開き`WebGL`を選択して`Switch Platform`ボタンをクリックします．  
![Switch Platform](https://user-images.githubusercontent.com/56796881/100954466-ee37e700-3557-11eb-83c8-b5dbd99df265.png)  
切り替えが完了すると`Switch Platform`ボタンのところが`Build`ボタンに変わるのでクリックしてビルドします．  

### Firebaseとの連携

[こちら](https://firebase.google.com/docs/web/setup?hl=ja)を参考にFirebaseプロジェクトの作成とアプリの登録を行ってください．Googleアカウントが必要です．  
ステップ2でアプリの登録が完了すると，Firebase SDKを追加する画面でスクリプトが表示されます．  
![Firebase SDK](https://user-images.githubusercontent.com/56796881/100954418-e415e880-3557-11eb-9872-4df9483bf912.png)  
このスクリプトを先ほどビルドしたフォルダ内にある`index.html`の<body>タグ内に追加してください．その時以下のコードも追加してください．  
`<script src="https://www.gstatic.com/firebasejs/8.1.1/firebase-firestore.js"></script>`  
![index.html](https://user-images.githubusercontent.com/56796881/100954422-e5dfac00-3557-11eb-9636-8508ad998ddd.png)  
次にCloud Firestoreというデータベースの作成を行います．  
![データベース作成](https://user-images.githubusercontent.com/56796881/100954426-e6784280-3557-11eb-8d28-bd7de5c22c2f.png)  
`本番環境`か`テストモード`を選べるので`テストモード`を選択し`次へ`をクリックしてください．  
![テストモード](https://user-images.githubusercontent.com/56796881/100954428-e710d900-3557-11eb-9b60-f1e6b8eb4488.png)  
Cloud Firestoreのロケーションは`asia-northeast2`を選択し`有効にする`をクリックしてください．
![ロケーション](https://user-images.githubusercontent.com/56796881/100955256-87b3c880-3559-11eb-891e-e479962d8914.png)  
`ルール`タブからルールの編集で以下のように変更し`公開`をクリックしてください．  
![ルール編集](https://user-images.githubusercontent.com/56796881/100954429-e7a96f80-3557-11eb-9a5c-f72402bb1a54.png)  

### Netlifyでサイトを公開  

GitHubで新しいリポジトリを作成し，ビルドして生成された`Buildフォルダ`，`TemplateDataフォルダ`，`index.html`をプッシュしてください．  
![リポジトリ作成](https://user-images.githubusercontent.com/56796881/100954431-e8420600-3557-11eb-8ad0-5c5448f7738f.png)  
[Netlify](https://app.netlify.com/)にアクセスし，`GitHub`をクリックして手順に従い登録をしてください．  
![Netlify登録](https://user-images.githubusercontent.com/56796881/100954432-e8420600-3557-11eb-8650-eff713c9a563.png)  
登録が完了すると以下のようなホーム画面に遷移するので`New site from Git`をクリックしてください．  
![Netlifyホーム画面](https://user-images.githubusercontent.com/56796881/100954434-e8da9c80-3557-11eb-875a-4f1d28b74238.png)  
`Github`をクリックしてください．  
![Netlify Step1](https://user-images.githubusercontent.com/56796881/100954436-e8da9c80-3557-11eb-8ebd-11fb8d34a9ea.png)  
自分のGithubのリポジトリ一覧が表示されるので，先程作成したリポジトリをクリックしてください．  
![Netlify Step2](https://user-images.githubusercontent.com/56796881/100954437-e9733300-3557-11eb-9e40-a1e680b928cc.png)  
`Deploy site`をクリックしてください．  
![Netlify Step3](https://user-images.githubusercontent.com/56796881/100954439-ea0bc980-3557-11eb-9afa-38fd9148f531.png)  
デプロイが完了するとサイトのURLが表示されます．  
![デプロイ完了](https://user-images.githubusercontent.com/56796881/100954442-ea0bc980-3557-11eb-915d-1f043e89a729.png)  

## ゲームの始め方

1.Netlifyで公開したサイトのURLにアクセスしゲームをロードします．スマートフォンの場合は，最初に`Please note that your browser is not currently supported for this Unity WebGL content. Press OK if you wish to continue anyway.`と表示されるので，`OK`ボタンを押してください．  
2.名前入力欄に名前を入力し，`接続`ボタンでステージに遷移します．スマートフォンの場合は，名前入力はできません．  
3.接続した全員が中央の`準備完了ボタン`をクリックすることでゲームがスタートします．  

### その他

名前入力欄に`admin`と入力し接続することでステージを監視することができます．矢印キー上でステージ全体と一部の画面の切り替えが可能です．また，ステージの一部を表示している画面では左右矢印キーで移動できます．タイトル画面に戻りたい場合は「0」キーで戻ることができます．  
![ステージ全画面](https://user-images.githubusercontent.com/56796881/100954443-eaa46000-3557-11eb-8e59-892a5fba7ae5.png)    

## 使用しているアセット

- [DOTween (HOTween v2) (Ver 1.2.420)](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)  
- [Sprite Pack #1 - Tap and Fly (Ver 1.1.0)](https://assetstore.unity.com/packages/2d/characters/sprite-pack-1-tap-and-fly-21454)  
- [PUN 2 無料版 (Ver 2.25.1)](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)  
- [Nature pixel art base assets FREE (Ver 1.0)](https://assetstore.unity.com/packages/2d/environments/nature-pixel-art-base-assets-free-151370)  
