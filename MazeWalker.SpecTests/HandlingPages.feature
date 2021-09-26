Feature: HandlingPages
Funda API paging is handled in a transparent way

Scenario: Results from all pages all aggregated
    Given there are following properties in funda on page 1
      | Address        | AgentId | AgentName     |
      | Hoofdweg 3     | 2       | Eva Makelaar  |
      | Kinkerstraat 1 | 1       | Adam Makelaar |
    And there are following properties in funda on page 2
      | Address        | AgentId | AgentName     |
      | Kerkstraat 2   | 1       | Adam Makelaar |
    When top agents are determined
    Then the top agents are the following
      | AgentId | AgentName     | TotalProperties |
      | 1       | Adam Makelaar | 2               |
      | 2       | Eva Makelaar  | 1               |