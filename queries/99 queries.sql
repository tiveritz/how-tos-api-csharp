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


# -----------------------------------------------------------------------------
#    1. Tester Queries
# -----------------------------------------------------------------------------
SELECT * FROM HowTos;
SELECT * FROM HowTosSteps;
SELECT * FROM HowTosUriIds;
SELECT * FROM Steps;
SELECT * FROM StepsUriIds;
SELECT * FROM Sub;
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
SELECT COUNT(step_id)
FROM Sub;


# -----------------------------------------------------------------------------
#    3. How To's Queries
# -----------------------------------------------------------------------------
# List all How To's
SELECT HowTosUriIds.uri_id, HowTos.title, HowTos.ts_create, HowTos.ts_update
FROM HowTos
JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id;

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
DELETE FROM HowTosUriIds
WHERE uri_id="a9d8cd7a";

# Change How To name
UPDATE HowTos
SET title = "changed title"
WHERE id = (
	SELECT how_to_id
	FROM HowTosUriIds
	WHERE uri_id="a9d8cd7a"
);


# -----------------------------------------------------------------------------
#    4. Steps Queries
# -----------------------------------------------------------------------------
# List all Steps
SELECT StepsUriIds.uri_id, Steps.title, Steps.ts_create, Steps.ts_update,
CASE
	WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super) THEN true ELSE false
END AS is_super
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id;

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
FROM Sub
WHERE step_id = '1';

# Delete Step
DELETE FROM StepsUriIds
WHERE uri_id="dj8d7f6e";

# Change Step name
UPDATE Steps
SET title = "changed title"
WHERE id = (
	SELECT step_id
	FROM StepsUriIds
	WHERE uri_id="dj8d7f6e"
);

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

# Check if a Step is linked to a Super
SELECT StepsUriIds.uri_id, Steps.title
FROM Steps
JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id
WHERE uri_id IN (
	SELECT super_id
    FROM Super
    WHERE step_id IN (
		SELECT step_id
        FROM StepsUriIds
        WHERE uri_id = "2ls98s7e"
    )
);


# -----------------------------------------------------------------------------
#    6. Order Queries
# -----------------------------------------------------------------------------
# Change Order of How To Steps
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

# Similar thing for SuperSteps...
