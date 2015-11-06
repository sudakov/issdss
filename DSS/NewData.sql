USE [issdss]
INSERT [dbo].[task] ([id], [name]) VALUES (6, N'TaskFuzzy')
GO
INSERT [dbo].[method] ([id], [name], [url]) VALUES (7, N'Нечеткий', N'Fuzzy')
GO
INSERT [dbo].[method] ([id], [name], [url]) VALUES (8, N'Минимум', NULL)
GO


INSERT [dbo].[criteria] ([id], [task_id], [name], [parent_crit_id], [rank], [ismin], [idealvalue], [method_id], [ord], [description], [is_number]) VALUES (125, 6, N'CritMin', NULL, CAST(0.000001 AS Decimal(18, 6)), 1, NULL, 7, 0, N'', 0)
GO
INSERT [dbo].[criteria] ([id], [task_id], [name], [parent_crit_id], [rank], [ismin], [idealvalue], [method_id], [ord], [description], [is_number]) VALUES (126, 6, N'Crit1', 125, CAST(0.250000 AS Decimal(18, 6)), 1, NULL, 7, 0, N'', 0)
GO
INSERT [dbo].[criteria] ([id], [task_id], [name], [parent_crit_id], [rank], [ismin], [idealvalue], [method_id], [ord], [description], [is_number]) VALUES (127, 6, N'Crit2', 125, CAST(0.250000 AS Decimal(18, 6)), 1, NULL, 7, 0, N'', 0)
GO
INSERT [dbo].[criteria] ([id], [task_id], [name], [parent_crit_id], [rank], [ismin], [idealvalue], [method_id], [ord], [description], [is_number]) VALUES (128, 6, N'Crit3', 125, CAST(0.500000 AS Decimal(18, 6)), 1, NULL, 7, 0, N'', 1)
GO
INSERT [dbo].[criteria] ([id], [task_id], [name], [parent_crit_id], [rank], [ismin], [idealvalue], [method_id], [ord], [description], [is_number]) VALUES (129, 6, N'Veliocity', 128, CAST(1.000000 AS Decimal(18, 6)), 1, NULL, 1, 0, N'', 1)
GO
INSERT [dbo].[crit_scale] ([id], [criteria_id], [name], [rank], [ord]) VALUES (250, 128, N'10', CAST(0.065055 AS Decimal(18, 6)), 1)
GO
INSERT [dbo].[crit_scale] ([id], [criteria_id], [name], [rank], [ord]) VALUES (251, 128, N'0', CAST(0.000000 AS Decimal(18, 6)), 0)
GO
INSERT [dbo].[crit_scale] ([id], [criteria_id], [name], [rank], [ord]) VALUES (252, 128, N'20', CAST(0.148698 AS Decimal(18, 6)), 2)
GO
INSERT [dbo].[crit_scale] ([id], [criteria_id], [name], [rank], [ord]) VALUES (253, 128, N'30', CAST(0.576208 AS Decimal(18, 6)), 3)
GO
INSERT [dbo].[crit_scale] ([id], [criteria_id], [name], [rank], [ord]) VALUES (254, 128, N'40', CAST(0.731115 AS Decimal(18, 6)), 4)
GO
INSERT [dbo].[crit_scale] ([id], [criteria_id], [name], [rank], [ord]) VALUES (255, 128, N'50', CAST(1.000000 AS Decimal(18, 6)), 5)
GO
INSERT [dbo].[alternative] ([id], [task_id], [name], [rank]) VALUES (231, 6, N'Alt1', CAST(0.117099 AS Decimal(18, 6)))
GO
INSERT [dbo].[alternative] ([id], [task_id], [name], [rank]) VALUES (232, 6, N'Alt2', CAST(0.500000 AS Decimal(18, 6)))
GO
INSERT [dbo].[alternative] ([id], [task_id], [name], [rank]) VALUES (233, 6, N'Alt3', CAST(0.123605 AS Decimal(18, 6)))
GO