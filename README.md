README – Labirintus metódusok
========================================
A program egy karakterekből felépített labirintust vizsgál.

Használt karakterek:
. = üres mező
█ = terem
╬ ═ ╦ ╩ ║ ╣ ╠ ╗ ╝ ╚ ╔ = járatelemek


GetRoomNumber(char[,] map)
========================================

Feladata:
Megszámolja a termeket a térképen.

Visszatérési érték:
int → a termek száma


GetSuitableEntrance(char[,] map)
========================================

Feladata:
Megszámolja a kijáratokat a pálya szélén.

Visszatérési érték:
int → kijáratok száma



IsInvalidElement(char[,] map)
========================================

Feladata:
Ellenőrzi, hogy van-e hibás karakter.

Visszatérési érték:
bool
true → van hibás karakter
false → nincs hibás karakter


GetUnavailableElements(char[,] map)
========================================

Feladata:
Megkeresi az elérhetetlen járatelemeket.

Visszatérési érték:
List<string> → koordináták listája


GenerateLabyrinth(List<string> positionsList)
========================================

Feladata:
Pozíciólista alapján labirintust generál.

Példa koordináta:
2:5

Visszatérési érték:
char[,] → elkészült térkép


IsPath(char c)
========================================

Feladata:
Megvizsgálja, hogy a karakter járat-e.

Visszatérési érték:
bool
