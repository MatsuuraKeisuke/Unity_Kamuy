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

## ONNXファイルメモ
| Kam_ver | メモ |
|---|---|
| Kam_ver5-1 | 8自由度トロット歩行 |
| Kam_ver6-1 | 右前足先負傷 |
| Kam_ver6-2 | 右前足全体負傷 |
| Kam_ver7 series | 12自由度トロット歩行 |
| 7-8 | sinndouを採用 |
| Kam_ver8 series | 12自由度トロット歩行 修正版 |
| 8-3 series | 12自由度トロット歩行 完成版 |
| Kam_ver9 | 右足前負傷 |
| 9-1 | ver11をモデルに |
| 9-2 | ver7-1をモデルに |
| Kam_ver10 | 右前　legだけ負傷 |
| Kam_ver10-1 | best |
| 10-2 | 右後 legだけ負傷 |
| Kam_ver11 | 右前 legだけ負傷：途中から |
| Kam_ver12 | 右前 全体負傷：途中から |
| Kam_ver12-1 | 右前 second_third free (ver12をモデルに) |
| 12-2 | 続き(12/13) |
| Kam_ver13 | 右後 legだけ負傷  (ver11をモデルに) |
| 13-1 | 続き |
| 13-2 | 右前 legだけ負傷：途中から　を学習　(ver13-1をモデルに) |
| 13-3 | 右後 legだけ負傷　(ver13-2をモデルに) |
| 13-4 | 続き　(ver13-3をモデルに) |
| Kam_ver14 | 右前　２箇所固定　(from ver8-3) |
| 14-1 | 続き(12/12) |
| Kam_ver15 | 右前 legだけ固定  (ver８−２をモデルに) |
| Kam_ver16 | 右後 ２箇所固定  (ver８−２をモデルに) |
| 16-1 | 続き(12/11) |
| 16-2 | 続き(12/13) |
| Kam_ver17 | 右後 second lock  (ver８−3をモデルに) |
| Kam_ver18 | 右後 third lock  (ver８−3をモデルに) |
| Kam_ver19 | 右前 second lock  (ver８−3をモデルに) |
| Kam_ver20 | 右前 second free  (ver８−3をモデルに) |
| Kam_ver21 | 右後 second free  (ver８−3をモデルに) |
| Kam_ver22 | 右後 second third free  (ver8-3をモデルに) |


## 今後の課題

- 床と足の摩擦を調整
- カムイの腕の位置を揃える
- 回転角度を 270 度に設定
- Controller を削除

