# Документация к игре "Морской бой".
# Техническое задание
1.	Общие требования    

    1.1. Игра должна быть реализована на языке C#.

    1.2. В качестве хранилища данных следует использовать .json файлы.

    1.3. Программа должна содержать три модуля: модуль DataLayer, модуль BusinessLayer и модуль PresentationLayer.

2.	Требования к модулю DataLayer

    2.1. Модуль должен предоставлять методы для чтения и записи данных в .json файлы.

    2.2. Модуль должен предоставлять методы для работы с таблицей лидеров и хранением игровых сессий.

3.	Требования к модулю BusinessLayer

    3.1. Модуль должен предоставлять методы для обработки хода игрока, проверки условий выигрыша и проигрыша.

    3.2. Модуль должен предоставлять методы для сохранения текущей игровой сессии и загрузки сохраненной сессии.

    3.3. Модуль должен предоставлять методы для обновления таблицы лидеров.

4.	Требования к модулю PresentationLayer

    4.1. Модуль должен предоставлять пользовательское меню с возможностью выбора команд: начало новой 
    игры, загрузка сохранения, выход из игры.

    4.2. Модуль должен выводить игровое поле.

    4.3. Модуль должен выводить результаты игрока.

#   Анализ задачи

1.	Разработка консольной игры "Морской бой":
Создание такого проекта подразумевает знание основ языка C#, умение работать с консольным вводом / выводом, работу с файлами, понимание принципов ООП и разделение программы на слои.

2.	Использование .json файлов для хранения данных:
Работа с .json файлами подразумевает умение сериализовать и десериализовать данные. В этом случае данные — это результаты игроков и состояние игровой сессии.

3.	Разработка трехслойной архитектуры:
Под этим требованием подразумевается создание трех отдельных модулей. Каждый модуль должен быть ответственным за свою зону обязанностей и не вмешиваться в обязанности других: DataAccessLayer работает с хранением и доступом к данным, LogicLayer оперирует логикой игры и бизнес-правилами, PresentationLayer отвечает за взаимодействие с пользователем.

4.	Реализация игровой механики "2048":
Игра "2048" имеет определенные правила, и на их базе необходимо реализовать алгоритм движения и слияния квадратов в зависимости от направления стрелки на клавише. Нужно также уметь обрабатывать ввод игрока с клавиатуры для управления игрой.

5.	Работа с таблицей лидеров и сохранением игры:
Это требование в себя включает сохранение и загрузку данных об игроках и их результатах, а также состояния игры для возможности продолжить игру в любой момент.

6.	Вывод информации на экран:
Необходимо организовать вывод меню, игрового поля и таблицы лидеров. Плюс обработка пользовательского ввода для взаимодействия с меню.

В целом задача достаточно сложная и требует применения различных навыков программирования. Требует внимания к деталям и умение работать со сложной структурой данных.

# Архитектура приложения
 Архитектура приложения будет состоять из трех слоев или модулей:
1.	DataLayer (Слой доступа к данным):
Этот слой отвечает за чтение и запись данных в JSON файлы. Он будет содержать классы Data и PlayerData для управления файлами, сериализации и десериализации данных.

2.	BusinessLayer (Функциональный слой):
На этом уровне реализуется весь функционал игры. Здесь будет класс "Functional", с методами для управления игровым процессом, как например, расстановка кораблей противника, расстановка моих кораблей, проверка на попадание и тд. 

3.	PresentationLayer (Слой представления):
Слой представления отвечает за взаимодействие с пользователем. Здесь будет класс "Menu", выводящий на экран различные команды и обрабатывающий ввод пользователя, и класс "Display", отображающий текущее состояние игры на экране, включая игровое поле и текущий счет.

Работа между слоями будет осуществляться следующим образом:

    PresentationLayer будет принимать ввод от пользователя, передавать его в LogicLayer для обработки и показывать результат, полученный от LogicLayer.

    LogicLayer будет интерпретировать команды от PresentationLayer, обрабатывать игровые сессии, сохранять и загружать состояние игры через DataAccessLayer.

    DataAccessLayer будет просто управлять файлами, сохранять и извлекать данные в/из JSON файлов по запросам от LogicLayer.
Такое разделение на слои помогает организовать код, делая его более читаемым и масштабируемым, так как каждый слой можно разрабатывать и тестировать независимо.

# Архитектура БД

В качестве БД был избран json формат файлов. Это связано с простотой взаимодействия с БД. 
Всего в приложении сохраняются 4 типа данных. Дата игры, время игры, имя пользователя и его результат.

Для качественной работы был выбран вариант хранения во временной папке системы, это позволит переносить программу по разным каталогам файловой системы, однако накопленные в базе данные никуда не потеряются.

# Листинг программы
Весь код можно прочесть в репозитории https://github.com/YaArtem333/sea-battle

