--Part 1
SELECT COLUMN_NAME, DATA_TYPE FROM
	INFORMATION_SCHEMA.COLUMNS
wHERE TABLE_NAME = 'jobs';

--Part 2
SELECT Name FROM Employers WHERE Location = "St. Louis City";

--Part 3
SELECT * FROM Skills
      LEFT JOIN JobSkills ON Skills.Id = JobSkills.SkillId
      WHERE JobSkills.JobId IS NOT NULL
      ORDER BY name DESC;
