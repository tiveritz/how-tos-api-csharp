SELECT * FROM HowTos;
SELECT * FROM HowTosSteps;
SELECT * FROM Steps;
SELECT * FROM Sub;
SELECT * FROM Super;


/* -- List all HowTos */
SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
FROM HowTos
JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id;

/* -- List specific HowTo */
SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
FROM HowTos
JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
WHERE uri_id="a9d8cd7a";

/* Get UriId from HowToId */
SELECT uri_id
FROM HowTosUriIds
WHERE how_to_id=1;

/* Get HowToId from UriId */
SELECT how_to_id
FROM HowTosUriIds
WHERE uri_id="a9d8cd7a";

/* -- List title, creation date, update date and if super of all Steps */
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id;

/* -- List title, creation date, update date and if super of a specific Steps */
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
WHERE uri_id="2ls98s7e";

/* Get StepId from UriId */
SELECT uri_id
FROM StepsUriIds
WHERE step_id=1;

/* Get UriId from StepId */
SELECT step_id
FROM StepsUriIds
WHERE uri_id="dj8d7f6e";

/* -- List all steps of a specific HowTo, order by position */
SELECT HowTosSteps.pos, StepsUriIds.uri_id, Steps.title,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
JOIN HowTosSteps ON HowTosSteps.step_id = Steps.id
WHERE HowTosSteps.how_to_id = (
    SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="a9d8cd7a")
ORDER BY HowTosSteps.pos;

/* -- List all substeps of a step */
SELECT Super.pos, Steps.title, Steps.ts_create, Steps.ts_update
FROM Super
JOIN Steps ON Super.step_id = Steps.id
WHERE Super.super_id = '2'
ORDER BY Super.pos;

/* -- Show the explanation of a specific step */
SELECT explanation
FROM Sub
WHERE step_id = '1';

/* -- Display the How Tos, where the step is linked */
SELECT HowTos.title
FROM HowTosSteps
JOIN HowTos ON HowTosSteps.how_to_id = HowTos.id
WHERE step_id = 1;

/* -- Check if a Step is linked to a Super */
SELECT Steps.title
FROM Super
JOIN Steps ON Super.super_id = Steps.id
WHERE step_id = 4;


/* -- Change Order of HowTosSteps */
SELECT *
FROM HowTosSteps
WHERE how_to_id = 1;

# Move from Top to Bottom -> Move 1 between 3 and 4
UPDATE HowTosSteps
SET pos = pos -1
WHERE how_to_id = 1 # currently selected howto
AND pos > 1 AND pos <= 3; # old position / new position
UPDATE HowTosSteps
SET pos = 3 # new position
WHERE how_to_id = 1 # currently selected howto
AND step_id = 1; # currently selected step

# Move from Bottom to Top -> Move 3 between 1 and 2
UPDATE HowTosSteps
SET pos = pos +1
WHERE how_to_id = 1 # currently selected howto
AND pos < 3 AND pos >= 1; # old position / new position
UPDATE HowTosSteps
SET pos = 1	# new position
WHERE how_to_id = 1 # currently selected howto
AND step_id = 7; # currently selected step

INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 1, 3); # Doesn't work

/* -- Change Order of Super */
SELECT * FROM Super
WHERE super_id = 2;

# should work exactly the same as for howtos...