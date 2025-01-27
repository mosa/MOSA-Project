##########
Unit tests
##########

The MOSA project has an extensive set of unit tests to help validate that the MOSA compiler is emitting correct binary code.

On Windows, execute the script ``Tests\RunAllUnitTestsWithPause.bat`` to run the unit tests.

On Linux, execute the following to run the unit tests:

.. code-block:: bash

	dotnet bin/Mosa.Utility.UnitTests.dll -oMax -check

The unit tests take a few minutes to execute on a modern PC. The results will be automatically displayed on the screen. The last line shows the total number of tests and failed tests, and the total time. Similar to the following:

.. code-block:: text

  Total Elapsed: 95.3 secs

  Unit Test Results:
     Passed:   68164
     Skipped:  4
     Failures: 0
     Total:    68168

  All unit tests passed successfully!
