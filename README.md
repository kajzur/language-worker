Language Worker 1.0
===============

Application is used to learning foreign words. This words we can group into, firstly some language (in default - English and German), and secondly into some groups. So, we have simple, hierarchical grouping, which is useful when we want to learn something special.
We provide posibility do create a groups, import words from file, delete groups or only empty it. Into empty group We can add words from files.

We can learn in 3 ways: 
===============

##### Tryb "Nauka do bólu"
Tryb w którym, jeśli nie odpowiemy na słówko, nie wypada ono z puli. Więc aby skończyć podejście, musimy odpowiedziec poprawnie na wszystkie słówka.

##### Tryb "Nauka w formie testu"
Tryb w którym mamy słówka na które nie odpowiemy, wypadają z puli. 

##### Tryb "Nauka początkowa"
Jest łatwiejszą wersją "Nauki do bólu", gdyż możemy, klikając w przycisk obok pola do wpisania słówka, uzyskać pierwszą literę, jako podpowiedź.

Statystyki nauki są zbierane tylko w trybie "Nauka w formie testu". 
W zakładce "Przegląd postępów" możemy dla każdej grupy wyświetlić wykres podejść oraz zapisać taki wykres do SVG.

## ChangeLog

v1.1.2
- Add posibility to learn from more than one group (ctrl+click)
- Change appearance
- Add error handling (eg. When some want to add group without name)
- Fix problem with windows. Now We have a hierarchical windows strategy
- Delete menu on top of window.
- Transfer some part of responsibility to DataBase 
- Refactoring
- Translate readme.md to English.

v1.0.1
- Fix bug with unwanted window, when user starts learning.
- Fix some typos.
