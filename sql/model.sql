CREATE TABLE task (
       id                   int NOT NULL,
       name                 varchar(255) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE person (
       id                   int NOT NULL,
       name                 varchar(255) NULL,
       login                varchar(30) NULL,
       password             varchar(255) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE person_task (
       id                   int NOT NULL,
       task_id              int NULL,
       person_id            int NULL,
       rank                 decimal(18,6) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE method (
       id                   int NOT NULL,
       name                 varchar(255) NULL,
       url                  varchar(20) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE criteria (
       id                   int NOT NULL,
       task_id              int NULL,
       name                 varchar(255) NULL,
       parent_crit_id       int NULL,
       rank                 decimal(18,6) NULL,
       ismin                int NULL,
       idealvalue           decimal(18,6) NULL,
       method_id            int NULL,
       ord                  int NULL,
       description          varchar(2000) NULL,
       is_number            int NULL,
	   code_model			int NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE crit_scale (
       id                   int NOT NULL,
       criteria_id          int NOT NULL,
       name                 varchar(255) NULL,
       rank                 decimal(18,6) NULL,
       ord                  int NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE preference (
       id                   int NOT NULL,
       criteria_id          int NOT NULL,
       crit_scale_id        int NOT NULL,
       parent_pref_id       int NULL,
       rank                 decimal(18,6) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE pair_crit_comp (
       id                   int NOT NULL,
       criteria1_id         int NOT NULL,
       criteria2_id         int NOT NULL,
       rank                 decimal(18,6) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE permission (
       id                   int NOT NULL,
       name                 varchar(255) NULL,
       pattern              varchar(255) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE role (
       id                   int NOT NULL,
       name                 varchar(255) NULL,
       task_id              int NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE role_permission (
       id                   int NOT NULL,
       permission_id        int NOT NULL,
       role_id              int NOT NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE person_role (
       id                   int NOT NULL,
       role_id              int NOT NULL,
       person_id            int NOT NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE alternative (
       id                   int NOT NULL,
       task_id              int NOT NULL,
       name                 varchar(255) NULL,
       rank                 decimal(18,6) NULL,
       model_id				int NULL,
	   trace_text			varchar(MAX) NULL,
	   model_name			varchar(255) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE crit_value (
       id                   int NOT NULL,
       person_id            int NULL,
       criteria_id          int NOT NULL,
       alternative_id       int NOT NULL,
       value                decimal(18,6) NULL,
       crit_scale_id        int NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE person_alternative (
       id                   int NOT NULL,
       alternative_id       int NULL,
       person_id            int NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE measure (
       id                   int NOT NULL,
       name                 varchar(30) NOT NULL,
       code                 varchar(30) NOT NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE resource (
       id                   int NOT NULL,
       name                 varchar(255) NOT NULL,
       task_id              int NULL,
       desrcription         varchar(2000) NULL,
       value                decimal(18,6) NOT NULL,
       period               int NULL,
       measure_id           int NULL,
       criteria_id          int NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE job (
       id                   int NOT NULL,
       name                 varchar(255) NOT NULL,
       alternative_id       int NULL,
       duration             decimal(18,6) NOT NULL,
       measure_id           int NOT NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE job_resource (
       id                   int NOT NULL,
       resource_id          int NOT NULL,
       job_id               int NOT NULL,
       value                decimal(18,6) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE plan_method (
       id                   int NOT NULL,
       name                 varchar(255) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE plandss (
       id                   int NOT NULL,
       plan_method_id       int NOT NULL,
       task_id              int NULL,
       name                 varchar(255) NULL,
       begin_date           datetime NOT NULL,
       end_date             datetime NOT NULL,
       measure_id           int NULL,
       isready              int NULL,
       alfa                 decimal(18,6) NULL,
       reserv_percent       decimal(18,4) NULL,
       func_value           decimal(18,6) NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE plan_job (
       id                   int NOT NULL,
       plan_id              int NOT NULL,
       job_id               int NOT NULL,
       begin_date           datetime NULL,
       end_date             datetime NULL,
       PRIMARY KEY (id)
)
go


CREATE TABLE job_loop (
       parent_job_id        int NOT NULL,
       child_job_id         int NOT NULL,
       PRIMARY KEY (parent_job_id, child_job_id)
)
go


CREATE TABLE plan_loop (
       parent_plan_id       int NOT NULL,
       child_plan_id        int NOT NULL,
       PRIMARY KEY (parent_plan_id, child_plan_id)
)
go


CREATE TABLE model(
		id				int NOT NULL,
		name			varchar(255) NOT NULL,
		description		varchar(2000) NULL,
		code			int NOT NULL,
		PRIMARY KEY (id, name)
		
)
go


CREATE TABLE model_crit(
		id_model 	int NOT NULL,
		id_crit		int NOT NULL,
		is_param	bit NOT NULL,
		code 		varchar(255) NOT NULL,
		PRIMARY KEY (id_crit)
)
go


ALTER TABLE model_crit
	   ADD FOREIGN KEY (id_model)
							 REFERENCES model
go


ALTER TABLE model_crit
	   ADD FOREIGN KEY (id_crit)
							 REFERENCES criteria
go


ALTER TABLE alternative
	   ADD FOREIGN KEY (model_id)
							 REFERENCES model
go


ALTER TABLE person_task
       ADD FOREIGN KEY (task_id)
                             REFERENCES task
go


ALTER TABLE person_task
       ADD FOREIGN KEY (person_id)
                             REFERENCES person
go


ALTER TABLE criteria
       ADD FOREIGN KEY (method_id)
                             REFERENCES method
go


ALTER TABLE criteria
       ADD FOREIGN KEY (parent_crit_id)
                             REFERENCES criteria
go


ALTER TABLE criteria
       ADD FOREIGN KEY (task_id)
                             REFERENCES task
go


ALTER TABLE crit_scale
       ADD FOREIGN KEY (criteria_id)
                             REFERENCES criteria
go


ALTER TABLE preference
       ADD FOREIGN KEY (parent_pref_id)
                             REFERENCES preference
go


ALTER TABLE preference
       ADD FOREIGN KEY (criteria_id)
                             REFERENCES criteria
go


ALTER TABLE preference
       ADD FOREIGN KEY (crit_scale_id)
                             REFERENCES crit_scale
go


ALTER TABLE pair_crit_comp
       ADD FOREIGN KEY (criteria2_id)
                             REFERENCES criteria
go


ALTER TABLE pair_crit_comp
       ADD FOREIGN KEY (criteria1_id)
                             REFERENCES criteria
go


ALTER TABLE role
       ADD FOREIGN KEY (task_id)
                             REFERENCES task
go


ALTER TABLE role_permission
       ADD FOREIGN KEY (permission_id)
                             REFERENCES permission
go


ALTER TABLE role_permission
       ADD FOREIGN KEY (role_id)
                             REFERENCES role
go


ALTER TABLE person_role
       ADD FOREIGN KEY (role_id)
                             REFERENCES role
go


ALTER TABLE person_role
       ADD FOREIGN KEY (person_id)
                             REFERENCES person
go


ALTER TABLE alternative
       ADD FOREIGN KEY (task_id)
                             REFERENCES task
go


ALTER TABLE crit_value
       ADD FOREIGN KEY (crit_scale_id)
                             REFERENCES crit_scale
go


ALTER TABLE crit_value
       ADD FOREIGN KEY (person_id)
                             REFERENCES person
go


ALTER TABLE crit_value
       ADD FOREIGN KEY (criteria_id)
                             REFERENCES criteria
go


ALTER TABLE crit_value
       ADD FOREIGN KEY (alternative_id)
                             REFERENCES alternative
go


ALTER TABLE person_alternative
       ADD FOREIGN KEY (alternative_id)
                             REFERENCES alternative
go


ALTER TABLE person_alternative
       ADD FOREIGN KEY (person_id)
                             REFERENCES person
go


ALTER TABLE resource
       ADD FOREIGN KEY (criteria_id)
                             REFERENCES criteria
go


ALTER TABLE resource
       ADD FOREIGN KEY (task_id)
                             REFERENCES task
go


ALTER TABLE resource
       ADD FOREIGN KEY (measure_id)
                             REFERENCES measure
go


ALTER TABLE job
       ADD FOREIGN KEY (alternative_id)
                             REFERENCES alternative
go


ALTER TABLE job
       ADD FOREIGN KEY (measure_id)
                             REFERENCES measure
go


ALTER TABLE job_resource
       ADD FOREIGN KEY (resource_id)
                             REFERENCES resource
go


ALTER TABLE job_resource
       ADD FOREIGN KEY (job_id)
                             REFERENCES job
go


ALTER TABLE plandss
       ADD FOREIGN KEY (task_id)
                             REFERENCES task
go


ALTER TABLE plandss
       ADD FOREIGN KEY (plan_method_id)
                             REFERENCES plan_method
go


ALTER TABLE plandss
       ADD FOREIGN KEY (measure_id)
                             REFERENCES measure
go


ALTER TABLE plan_job
       ADD FOREIGN KEY (plan_id)
                             REFERENCES plandss
go


ALTER TABLE plan_job
       ADD FOREIGN KEY (job_id)
                             REFERENCES job
go


ALTER TABLE job_loop
       ADD FOREIGN KEY (parent_job_id)
                             REFERENCES job
go


ALTER TABLE job_loop
       ADD FOREIGN KEY (child_job_id)
                             REFERENCES job
go


ALTER TABLE plan_loop
       ADD FOREIGN KEY (parent_plan_id)
                             REFERENCES plandss
go


ALTER TABLE plan_loop
       ADD FOREIGN KEY (child_plan_id)
                             REFERENCES plandss
go
