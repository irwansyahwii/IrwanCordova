Feature: Device Module

@mytag
Scenario: Machine Id key is not exists and a newly generated MachineId will be stored in the registry
	Given This GUID "C62A91D0-482A-449B-B955-D7911FEB5A8F"
	And Machine id key is not exists
	When Device.AcquireUniqueId() is called
	Then A new key with name MachineId must be created in registry \CurrentUser\Software\Intel\Cordova 
	And it's value must be set to "C62A91D0-482A-449B-B955-D7911FEB5A8F"
	And The Device.UniqueId property value must be the same as "C62A91D0-482A-449B-B955-D7911FEB5A8F"

@mytag
Scenario: Machine id key is already exists, don't overwrite the existing MachineID
	Given This GUID "2B17C4B0-4999-4CFE-AB3B-C7882D4395B6"
	And MachineID key is exists with this GUID "6DEA3BB5-DBCF-4497-899C-61DED0011606"
	When Device.AcquireUniqueId() is called
	Then Existing MachineID must be the same as "6DEA3BB5-DBCF-4497-899C-61DED0011606"
	And The Device.UniqueId property value must be the same as "6DEA3BB5-DBCF-4497-899C-61DED0011606"


Scenario: Passing action "getDeviceInfo" the matching method must be called
	Given This action "getDeviceInfo" and callback id "1"
	When Device.Execute is called
	Then Device.GetDeviceInfo() must be called with callback id "1"
	And Success callback is called with the correct result

