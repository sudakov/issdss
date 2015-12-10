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

INSERT method (id, name, url) VALUES (7, 'Нечеткий', 'Fuzzy')
go
INSERT method (id, name, url) VALUES (8, 'MIN', 'FuzzyMin')
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

-------------------------------------------------- модели и параметры

--создаем пару тестовых задач
DECLARE @id int
SET @id = isnull((SELECT max(id) FROM issdss.dbo.task),0) + 1
INSERT INTO issdss.dbo.task (id, name)
SELECT @id, 'Тест с СМО'
UNION
SELECT @id + 1, 'Тест с вычислительной сетью'
GO

--записываем пару тестовых моделей
INSERT INTO issdss.dbo.model (id, name, description, code)
SELECT 1, 'Простая СМО', 'Простая СМО с двумя входными потоками', 1
UNION
SELECT 2, 'Вычислительная сеть', 'Модель вычислительной сети', 2
GO

--записываем альтернативу для простой СМО
DECLARE @t_id int
SET @t_id = isnull((SELECT max(id) FROM issdss.dbo.task),0) - 1
DECLARE @a_id int
SET @a_id = isnull((SELECT max(id) FROM issdss.dbo.alternative),0) + 1
INSERT INTO issdss.dbo.alternative (id, task_id, name, model_id, trace_text, model_name)
VALUES (@a_id, @t_id, 'первый прогон', 1, 'trace', 'Простая СМО')
GO

--параметры СМО
DECLARE @t_id int
SET @t_id = isnull((SELECT max(id) FROM issdss.dbo.task),0) - 1
DECLARE @c_id int
SET @c_id = isnull((SELECT max(id) FROM issdss.dbo.criteria),0) + 1
INSERT INTO issdss.dbo.criteria (id, task_id, name, is_number)
SELECT @c_id, @t_id, 'длина очереди 1', 1
UNION
SELECT @c_id + 1, @t_id, 'длина очереди 2', 1
UNION
SELECT @c_id + 2, @t_id, 'длина очереди 3', 1
UNION
SELECT @c_id + 3, @t_id, 'интенсивность входа 1', 1
UNION
SELECT @c_id + 4, @t_id, 'интенсивность входа 2', 1
UNION
SELECT @c_id + 5, @t_id, 'МО интенсивности обслуживания 1', 1
UNION
SELECT @c_id + 6, @t_id, 'МО интенсивности обслуживания 2', 1
UNION
SELECT @c_id + 7, @t_id, 'вероятность необходимости дообслуживания', 1
UNION
SELECT @c_id + 8, @t_id, 'время прогона', 1
UNION
SELECT @c_id + 9, @t_id, 'средняя длина очереди 1', 1
UNION
SELECT @c_id + 10, @t_id, 'средняя длина очереди 2', 1
UNION
SELECT @c_id + 11, @t_id, 'средняя длина очереди 3', 1
UNION
SELECT @c_id + 12, @t_id, 'частота потери заявок в очереди 1', 1
UNION
SELECT @c_id + 13, @t_id, 'частота потери заявок в очереди 2', 1
UNION
SELECT @c_id + 14, @t_id, 'частота потери заявок в очереди 3', 1
UNION
SELECT @c_id + 15, @t_id, 'Оценка вероятности занятости КО 1', 1
UNION
SELECT @c_id + 16, @t_id, 'Оценка вероятности занятости КО 2', 1
--численные значения
DECLARE @v_id int
SET @v_id = isnull((SELECT max(id) FROM issdss.dbo.crit_value),0) + 1
DECLARE @a_id int
SET @a_id = isnull((SELECT max(id) FROM issdss.dbo.alternative),0)
INSERT INTO issdss.dbo.crit_value (id, criteria_id, alternative_id, value)
SELECT @v_id, @c_id, @a_id, 3.0
UNION
SELECT @v_id + 1, @c_id + 1, @a_id, 4.0
UNION
SELECT @v_id + 2, @c_id + 2, @a_id, 5.0
UNION
SELECT @v_id + 3, @c_id + 3, @a_id, 0.003
UNION
SELECT @v_id + 4, @c_id + 4, @a_id, 0.002
UNION
SELECT @v_id + 5, @c_id + 5, @a_id, 0.004
UNION
SELECT @v_id + 6, @c_id + 6, @a_id, 0.006
UNION
SELECT @v_id + 7, @c_id + 7, @a_id, 0.25
UNION
SELECT @v_id + 8, @c_id + 8, @a_id, 144400.0
GO
















