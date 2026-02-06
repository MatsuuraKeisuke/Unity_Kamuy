# Unity_Kamuy

## インストール方法
参考リンク
- Unity / ML-Agents セットアップ  
  https://note.com/npaka/n/n30707ea92bd1
- Python 仮想環境の作成  
  https://note.com/npaka/n/n096f810f36a6


### 手順
1. Unity Hub をダウンロード
2. ML-Agents をダウンロード  
   - ml-agents-release_19
3. Python の仮想環境を準備

仮想環境の有効化
```,
    source env/bin/activate
```
仮想環境の無効化
```
    deactivate
```
おすすめエイリアス
```bash
    alias env='cd ~/ML-Agents && source env/bin/activate && cd ml-agents-release_19'
```

## 今後の課題

- 床と足の摩擦を調整
- カムイの腕の位置を揃える
- 回転角度を 270 度に設定
- Controller を削除

