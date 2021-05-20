#  ____________________________________
# |    _   _             _ _           |
# |   | |_(_)   _ __ _ _(_) |_ ____    |
# |   | __| |\ / /_ \ '_| | __|_  /    |
# |   | |_| | ' / __/ | | | |__/ /_    |
# |    \__|_|\_/\___|_| |_|\__/___/    |
# |____________________________________|
#
# 1. Tester Queries
# 2. Statistics Queries
# 3. How To's Queries
# 4. Steps Queries
# 5. Data Integrity Queries
# 6. Order Queries
# 6.1 Change Order of How To's
# 6.2 Change Order of Substeps


# -----------------------------------------------------------------------------
#    1. Tester Queries
# -----------------------------------------------------------------------------
SELECT * FROM HowTos;
SELECT * FROM HowTosSteps;
SELECT * FROM HowTosUriIds;
SELECT * FROM Steps;
SELECT * FROM StepsUriIds;
SELECT * FROM StepsExplanations;
SELECT * FROM Super;


# -----------------------------------------------------------------------------
#    2. Statistics Queries
# -----------------------------------------------------------------------------
# Count How To's
SELECT COUNT(id) as how_tos_count
FROM HowTos;

# Count Steps
SELECT COUNT(id) as steps_count
FROM Steps;

# Count SuperSteps
SELECT COUNT(DISTINCT(super_id))
FROM Super;

# Count SubSteps
SELECT COUNT(id) as substeps_count
FROM Steps
WHERE id NOT IN (
	SELECT DISTINCT step_id
    FROM Super
);


# -----------------------------------------------------------------------------
#    3. How To's Queries
# -----------------------------------------------------------------------------
# List all How To's
SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
FROM HowTos
JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
ORDER BY ts_update DESC;

# List a specific How To by uri_id
SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
FROM HowTos
JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
WHERE uri_id="a9d8cd7a";

# Get uri_id from how_to_id
SELECT uri_id
FROM HowTosUriIds
WHERE how_to_id=1;

# Get how_to_id from uri_id
SELECT how_to_id
FROM HowTosUriIds
WHERE uri_id="a9d8cd7a";

# Delete How To
# All connected tables are cascade deleted
DELETE FROM HowTos
WHERE id = (
	SELECT how_to_id
    FROM HowTosUriIds
    WHERE uri_id="a9d8cd7a"
);

# Change How To name
UPDATE HowTos
SET title = "changed title"
WHERE id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="a9d8cd7a"
);

# Link Step to How To   
INSERT INTO HowTosSteps
SET
	how_to_id = (
		SELECT how_to_id
		FROM HowTosUriIds
		WHERE uri_id="a9d8cd7a"
	),
	step_id = (
		SELECT step_id
		FROM StepsUriIds
		WHERE uri_id="d874djd9"
	),
	pos = (
		CASE
			WHEN ( # WHEN no steps are linked, position = 0
				SELECT COUNT(step_id)
					FROM HowTosSteps hts
					WHERE how_to_id = (
						SELECT how_to_id
						FROM HowTosUriIds
						WHERE uri_id="a9d8cd7a"
					)
				) = 0
			THEN 0
			ELSE ( # WHEN at least one step is linked, assign new pos
			SELECT MAX(pos) + 1
				FROM HowTosSteps hts
				WHERE how_to_id = (
					SELECT how_to_id
					FROM HowTosUriIds
					WHERE uri_id="a9d8cd7a"
					)
                )
		END
	);

# List all Steps that can be added
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
WHERE id NOT IN (
	SELECT id
	FROM Steps
	JOIN HowTosSteps ON HowTosSteps.step_id = Steps.id
	WHERE HowTosSteps.how_to_id = (
		SELECT how_to_id
		FROM HowTosUriIds
		WHERE uri_id="a9d8cd7a")
	ORDER BY HowTosSteps.pos);



# -----------------------------------------------------------------------------
#    4. Steps Queries
# -----------------------------------------------------------------------------
# List all Steps
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
ORDER BY ts_update DESC;

# List specific Step
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
WHERE uri_id="2ls98s7e";

# Get uri_id from step_id
SELECT uri_id
FROM StepsUriIds
WHERE step_id=1;

# Get step from uri_id
SELECT step_id
FROM StepsUriIds
WHERE uri_id="dj8d7f6e";

# List all Steps of a specific How To by uri_id, order by position
SELECT StepsUriIds.uri_id, Steps.title,
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

# List all SubSteps of a Step
SELECT StepsUriIds.uri_id, Steps.title,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
JOIN Super ON Super.step_id = Steps.id
WHERE Super.super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
ORDER BY Super.pos;

# Show the explanation of a specific step
SELECT explanation
FROM StepsExplanations
WHERE step_id = '1';

# Delete Step
# All connected tables are cascade deleted
DELETE FROM Steps
WHERE id = (
	SELECT step_id
    FROM StepsUriIds
    WHERE uri_id="dj8d7f6e"
);

# Change Step name
UPDATE Steps
SET title = "changed title"
WHERE id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="dj8d7f6e"
);

# Link Step to Super 
INSERT INTO Super
SET
	super_id = (
		SELECT step_id
		FROM StepsUriIds
		WHERE uri_id="d874djd9"
	),
	step_id = (
		SELECT step_id
		FROM StepsUriIds
		WHERE uri_id="2ls98s7e"
	),
	pos = (
		CASE
			WHEN ( # WHEN no steps are linked, position = 0
				SELECT COUNT(step_id)
					FROM Super s
						WHERE super_id = (
							SELECT step_id
							FROM StepsUriIds
							WHERE uri_id="d874djd9"
						)
				) = 0
			THEN 0
			ELSE ( # WHEN at least one step is linked, assign new pos
			SELECT MAX(pos) + 1
				FROM Super s
					WHERE super_id = (
						SELECT step_id
						FROM StepsUriIds
						WHERE uri_id="d874djd9"
					)
                )
		END
    );


# List all Steps that can be added
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
WHERE id NOT IN ( # can not link direct children
	SELECT id
	FROM Steps
	JOIN Super ON Super.step_id = Steps.id
	WHERE Super.super_id = (
		SELECT step_id
		FROM StepsUriIds
		WHERE uri_id="2ls98s7e"
        )
	)
AND id != (
		SELECT step_id
		FROM StepsUriIds
		WHERE uri_id="2ls98s7e"
        )
ORDER BY ts_update DESC;


# -----------------------------------------------------------------------------
#    5. Data Integrity Queries
# -----------------------------------------------------------------------------
# Display the How Tos, where the step is linked
SELECT HowTosUriIds.uri_id, HowTos.title
FROM HowTos
JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id
WHERE HowTos.id IN (
	SELECT how_to_id
    FROM HowTosSteps
    WHERE step_id = (
		SELECT step_id
		FROM StepsUriIds
		WHERE uri_id="a93jdjc7"
    )
);

# Get uriIds of Supersteps
SELECT StepsUriIds.uri_id
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
WHERE id IN (
SELECT super_id
    FROM Super
    WHERE step_id IN (
		SELECT step_id
        FROM StepsUriIds
        WHERE uri_id = "a00d9s8e"
    )
);

# List uriIds of Substeps
SELECT StepsUriIds.uri_id
FROM Steps
JOIN StepsUriIds ON StepsUriIds.step_id = Steps.id
JOIN Super ON Super.step_id = Steps.id
WHERE Super.super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="d874djd9")
ORDER BY Super.pos;

# -----------------------------------------------------------------------------
#    6. Order Queries
#    6.1 Change Order of How To Steps
# -----------------------------------------------------------------------------
# Move step down -> Move 0 to 2
SELECT @step:=step_id
FROM HowTosSteps
WHERE how_to_id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="a9d8cd7a")
    AND pos = 0;
# then
UPDATE HowTosSteps
SET pos = pos - 1
WHERE how_to_id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="a9d8cd7a") # currently selected howto
AND pos > 0 AND pos <= 2; # old position / new position
# then
UPDATE HowTosSteps
SET pos = 2 # new position
WHERE how_to_id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="a9d8cd7a") # currently selected howto
AND step_id = @step; # currently selected step

# Move step up -> Move 2 to 0
SELECT @step:=step_id
FROM HowTosSteps
WHERE how_to_id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="l9d86e5s")
    AND pos = 2;
# then    
UPDATE HowTosSteps
SET pos = pos + 1
WHERE how_to_id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="l9d86e5s") # currently selected howto
AND pos < 2 AND pos >= 0; # old position / new position
# then
UPDATE HowTosSteps
SET pos = 0	# new position
WHERE how_to_id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="l9d86e5s") # currently selected howto
AND step_id = @step;  # currently selected step


# -----------------------------------------------------------------------------
#    6.2 Change Order of Substeps djc847dj
# -----------------------------------------------------------------------------
# Move step down -> Move 0 to 1
SELECT @step:=step_id
FROM Super
WHERE super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
    AND pos = 0;
# then
UPDATE Super
SET pos = pos - 1
WHERE super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
AND pos > 0 AND pos <= 1;
# then
UPDATE Super
SET pos = 1
WHERE super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
AND step_id = @step;

# Move step up -> Move 1 to 0
SELECT @step:=step_id
FROM Super
WHERE super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
    AND pos = 1;
# then    
UPDATE Super
SET pos = pos + 1
WHERE super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
AND pos < 1 AND pos >= 0;
# then
UPDATE Super
SET pos = 0
WHERE super_id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="djc847dj")
AND step_id = @step;
