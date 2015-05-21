INSERT INTO method (id, name)
SELECT 1, 'Взвешенная сумма (непосредственное назначение)'
UNION 
SELECT 2, 'Взвешенная сумма (парные сравнения)'
UNION 
SELECT 3, 'Идеальная точка'
UNION 
SELECT 4, 'Функция предпочтений'
UNION 
SELECT 5, 'Медиана Кемени'
UNION
SELECT 6, 'Мультипликативная свёртка'
go


INSERT INTO [issdss].[dbo].[permission] (id, name, pattern)
SELECT  1,    'Просмотр задач',    NULL
UNION
SELECT  2,    'Добавление задач',    NULL
UNION
SELECT  3,    'Редактирование задач',    NULL
UNION
SELECT  4,    'Удаление задач',    NULL
UNION
SELECT 11,    'Просмотр альтернатив',    NULL
UNION
SELECT 12,    'Добавление альтернатив',    NULL
UNION
SELECT 13,    'Редактрирование альтернатив',    NULL
UNION
SELECT 14,    'Удаление альтернатив',        NULL
UNION
SELECT 21,    'Просмотр критериев',        NULL
UNION
SELECT 22,    'Добавление критериев',        NULL
UNION
SELECT 23,    'Редактирование критериев',    NULL
UNION
SELECT 24,    'Удаление критериев',        NULL
UNION
SELECT 31,    'Просмотр экспертных оценок',    NULL
UNION
SELECT 32,    'Проведение экспертизы',    NULL
UNION
SELECT 41,    'Просмотр результатов ранжирования',    NULL
UNION
SELECT 42,    'Проведение ранжирования',        NULL
UNION
SELECT 51,    'Просмотр пользователей',        NULL
UNION
SELECT 52,    'Редактирование пользователей',        NULL
UNION
SELECT 53,    'Просмотр задач пользователей',    NULL
UNION
SELECT 54,    'Назначение задач пользователям',    NULL
UNION
SELECT 55,    'Просмотр альтернатив пользователей',    NULL
UNION
SELECT 56,    'Назначение альтернатив пользователям',    NULL
UNION
SELECT 57,    'Просмотр ролей пользователей',    NULL
UNION
SELECT 58,    'Назначение ролей пользователям',    NULL
UNION
SELECT 59,    'Удаление пользователей',        NULL
UNION
SELECT 61,    'Просмотр ролей',            NULL
UNION
SELECT 62,    'Добавление ролей',        NULL
UNION
SELECT 63,    'Редактирование ролей',        NULL
UNION
SELECT 64,    'Просмотр разрешений ролей',    NULL
UNION
SELECT 65,    'Назначение разрешений ролям',    NULL
UNION
SELECT 66,    'Удаление ролей',        NULL
UNION
SELECT 71,    'Глобальный просмотр среди всех задач',        NULL
UNION
SELECT 81,    'Глобальный просмотр среди всех пользователей',    NULL
GO

insert into measure (id, name, code)
select 1, 'год', 'year'
union
select 2, 'квартал', 'quarter'
union
select 3, 'месяц', 'month'
union
select 4, 'день', 'day'
GO 

insert into plan_method (id, name)
select 1, 'Ручной ввод'
union
select 2, 'Жадный алгоритм'
union
select 3, 'НПВР'

GO