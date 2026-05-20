const translations = {
    hu: {
        title: 'Labirintus Játék Dokumentáció',
        subtitle: 'Konzolos C# projekt bemutatása',
        introTitle: 'A projekt célja',
        introText: 'A játék célja a labirintus összes kincstermének felfedezése, majd a kijáraton történő kijutás. A játék konzolos felületen működik és a játékos WASD billentyűkkel mozog.',
        introTextCopy: 'Fedezd fel a termeket, jegyezd meg az útvonalakat, majd találd meg a kijáratot. A dokumentáció röviden bemutatja a játék működését, irányítását és a megvalósított metódusokat.',
        teamTitle: 'A projektmunka',
        teamText: 'A projektmunkát 3 fő végezte a következő bontásban:',
        member1: 'Labirintus játék elkészítése',
        member2: 'Térképszerkesztő elkészítése',
        member3: 'Megadott metódusok implementálása és a weblap elkészítése: Jakab Tamás',
        controlsTitle: 'Irányítás',
        northText: 'Mozgás északra',
        westText: 'Mozgás nyugatra',
        southText: 'Mozgás délre',
        eastText: 'Mozgás keletre',
        mapTitle: 'Térkép karakterek',
        wallText: 'Fal / kitöltés',
        roomText: 'Kincses terem',
        pathText: 'Járatok',
        saveTitle: 'Mentés és visszatöltés',
        saveText: 'A játék támogatja a mentést és a visszatöltést. A mentések .SAV kiterjesztésű fájlokba kerülnek.',
        tipsTitle: 'Tippek',
        tip1: 'Mindig figyeld a lehetséges irányokat.',
        tip2: 'Először próbáld megtalálni az összes termet.',
        tip3: 'Fedett térképnél jegyezd meg az útvonalakat.',
        methodsTitle: 'Megvalósított metódusok',
        method1: 'Termek megszámolása',
        method2: 'Kijáratok keresése',
        method3: 'Hibás karakterek ellenőrzése',
        method4: 'Elérhetetlen járatok keresése',
        method5: 'Labirintus generálása',
        footerText: 'Készítette: Jakab Tamás'
    },
    en: {
        title: 'Labyrinth Game Documentation',
        subtitle: 'Console C# project presentation',
        introTitle: 'Project Goal',
        introText: 'The goal of the game is to discover every treasure room and escape through an exit. The game runs in the console and the player moves with the WASD keys.',
        introTextCopy: 'Explore the rooms, remember the routes, then find the exit. This documentation briefly presents the gameplay, controls, and implemented methods.',
        teamTitle: 'Project Work',
        teamText: 'The project was completed by 3 members with the following responsibilities:',
        member1: 'Creating the labyrinth game',
        member2: 'Creating the map editor',
        member3: 'Implementing the required methods and building the website: Tamas Jakab',
        controlsTitle: 'Controls',
        northText: 'Move north',
        westText: 'Move west',
        southText: 'Move south',
        eastText: 'Move east',
        mapTitle: 'Map Characters',
        wallText: 'Wall / filler',
        roomText: 'Treasure room',
        pathText: 'Paths',
        saveTitle: 'Save and Load',
        saveText: 'The game supports saving and loading. Save files use the .SAV extension.',
        tipsTitle: 'Tips',
        tip1: 'Always check the available directions.',
        tip2: 'Try to find every treasure room first.',
        tip3: 'Memorize routes when using hidden map mode.',
        methodsTitle: 'Implemented Methods',
        method1: 'Count rooms',
        method2: 'Find exits',
        method3: 'Check invalid characters',
        method4: 'Find unreachable paths',
        method5: 'Generate labyrinth',
        footerText: 'Created by: Tamas Jakab'
    }
};

function setLanguage(lang) {
    const selected = translations[lang] || translations.hu;

    Object.entries(selected).forEach(([id, value]) => {
        const element = document.getElementById(id);
        if (element) {
            element.innerText = value;
        }
    });

    document.documentElement.lang = lang;

    document.querySelectorAll('[data-lang]').forEach((button) => {
        button.classList.toggle('active', button.dataset.lang === lang);
    });
}

document.querySelectorAll('[data-lang]').forEach((button) => {
    button.addEventListener('click', () => setLanguage(button.dataset.lang));
});