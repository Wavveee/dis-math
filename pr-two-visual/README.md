# ProjectTwo Visualization (pr-two-visual)

This Python project provides visual representations of the C# ProjectTwo set operations program.

## What is ProjectTwo?

ProjectTwo is a C# console application that performs set operations:
- **Difference A \ B**: Elements in A but not in B
- **Difference B \ C**: Elements in B but not in C  
- **Union (A \ B) ∪ (B \ C)**: Combined result of both differences

## Visualization Features

The Python visualization creates:

1. **Venn Diagram** (`venn_diagram_visualization.png`)
   - Visual representation of sets A, B, C within universal set U
   - Four subplots showing:
     - The universal set with all three sets
     - Operation 1: A \ B (highlighted)
     - Operation 2: B \ C (highlighted)
     - Final result: (A \ B) ∪ (B \ C)

2. **Project Information** (`project_info_visualization.png`)
   - Project structure and metadata
   - Key methods and operations
   - Framework and language details

## Installation & Usage

**Prerequisites:** Python 3.7+

**Install dependencies:**
```bash
pip install -r requirements.txt
```

**Run the visualization:**
```bash
python visualize_project.py
```

This will:
- Generate two PNG images with visualizations
- Display them in interactive matplotlib window
- Save them to the current directory

## Output Files

- `venn_diagram_visualization.png` - Venn diagrams showing set operations
- `project_info_visualization.png` - Project structure and details

## Example

With sets:
- U = {1, 2, 3, 4, 5, 6, 7, 8, 9}
- A = {1, 2, 3, 4, 5}
- B = {3, 4, 5, 6, 7}
- C = {5, 6, 7, 8, 9}

Results:
- A \ B = {1, 2}
- B \ C = {3, 4}
- (A \ B) ∪ (B \ C) = {1, 2, 3, 4}

## Author

Created as visualization for ProjectTwo C# set operations program.
