# README – Labirintus metódusok
========================================


## Program leírása
========================================


Ez a program egy karakterekből felépített labirintust vizsgál.
A térkép egy `char[,]` típusú kétdimenziós tömbben van tárolva.

A program képes:

* megszámolni a termeket,
* megszámolni a kijáratokat,
* ellenőrizni a hibás karaktereket,
* megkeresni az elérhetetlen járatelemeket,
* valamint új labirintust generálni pozíciólista alapján.

---

## Használt karakterek
========================================


| Karakter                | Jelentés    |
| ----------------------- | ----------- |
| `.`                     | üres mező   |
| `█`                     | terem       |
| `╬ ═ ╦ ╩ ║ ╣ ╠ ╗ ╝ ╚ ╔` | járatelemek |

---

## Metódusok

### `GetRoomNumber(char[,] map)`

Megszámolja, hogy hány terem (`█`) található a térképen.

Visszatérési érték:

* teremszám (`int`)

---

### `GetSuitableEntrance(char[,] map)`

Megszámolja, hogy hány kijárat található a labirintus szélén.

Visszatérési érték:

* kijáratok száma (`int`)

---

### `IsInvalidElement(char[,] map)`

Ellenőrzi, hogy van-e szabálytalan karakter a térképen.

Visszatérési érték:

* `true` → van hibás karakter
* `false` → minden karakter érvényes

---

### `GetUnavailableElements(char[,] map)`

Megkeresi azokat a járatelemeket, amelyekhez nem kapcsolódik másik járat.

Visszatérési érték:

* elérhetetlen elemek listája (`List<string>`)

---

### `GenerateLabyrinth(List<string> positionsList)`

Új labirintust generál egy pozíciólista alapján.

A lista elemei ilyen formátumúak:
`"sor:oszlop"`

Példa:
`"2:5"`

Visszatérési érték:

* elkészült labirintus (`char[,]`)

---

### `IsPath(char c)`

Megvizsgálja, hogy egy karakter járatelem-e.

Visszatérési érték:

* `true` → járat
* `false` → nem járat

---

## Program működése

A program egy előre definiált `testMap` térképpel indul.

A `Main()` függvény meghívja a különböző metódusokat, majd kiírja az eredményeket:

1. termek száma
2. kijáratok száma
3. hibás karakterek ellenőrzése
4. elérhetetlen elemek listája

---

## Megjegyzés

A program konzolos alkalmazásként készült C# nyelven.
