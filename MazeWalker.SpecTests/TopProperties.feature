Feature: TopProperties
	Top agents are identified from a list of property listings

Scenario: Agents with most properties are identified and listed in order of properties listed
	Given there are following properties in funda
	| Address        | AgentId | AgentName     |
	| Hoofdweg 3     | 2       | Eva Makelaar  |
	| Kinkerstraat 1 | 1       | Adam Makelaar |
	| Kerkstraat 2   | 1       | Adam Makelaar |
	When top agents are determined
	Then the top agents are the following
	| AgentId | AgentName     | TotalProperties |
	| 1       | Adam Makelaar | 2               |
	| 2       | Eva Makelaar  | 1               |