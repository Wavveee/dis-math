import matplotlib.pyplot as plt
import matplotlib.patches as patches
import numpy as np
from matplotlib.patches import Circle
from pathlib import Path
import matplotlib as mpl

# ==========================================
# НАЛАШТУВАННЯ
# ==========================================
OUTPUT_DIR = Path(r"E:\NUWGP\dis-math\pr-two-visual\img")

# Стиль
HATCH_PATTERN = '///'
HATCH_COLOR = '#0099cc'
OUTLINE_COLOR = 'black'
BG_COLOR = 'white'
SKETCH_OPTS = (1.5, 128, 4) # Параметри "малювання від руки"

def get_hardcoded_data():
    return {
        "U": {'1', '2', '3', '4', '5', '6', '7', '8', '9'},
        "A": {'1', '2', '3', '4', '5', '55'},
        "B": {'3', '4', '5', '6', '7'},
        "C": {'5', '6', '7', '8', '9'},
        "formula": r"(A \setminus B) \cup (B \setminus C)"
    }

def setup_ax_style(ax, title):
    ax.set_xlim(-1.6, 1.6)
    ax.set_ylim(-1.6, 1.6)
    ax.set_aspect('equal')
    ax.axis('off')
    ax.text(0, -1.8, title, ha='center', fontsize=12, color=OUTLINE_COLOR)

# --- МЕТОД 1: ХУДОЖНІ ШАРИ (Для простих операцій) ---
def draw_sketch_circle(ax, center, radius, label=None, layer='outline'):
    if layer == 'hatch':
        # Шар штрихування
        patch = Circle(center, radius, facecolor='none', edgecolor=HATCH_COLOR,
                       hatch=HATCH_PATTERN, linewidth=0, zorder=1)
        ax.add_patch(patch)
    elif layer == 'mask':
        # Шар маскування (білий, перекриває нижній шар)
        patch = Circle(center, radius, facecolor=BG_COLOR, edgecolor='none', zorder=2)
        ax.add_patch(patch)
    elif layer == 'outline':
        # Шар контуру
        patch = Circle(center, radius, facecolor='none', edgecolor=OUTLINE_COLOR,
                       linewidth=2, zorder=3)
        patch.set_sketch_params(*SKETCH_OPTS)
        ax.add_patch(patch)
        
        if label:
            lx, ly = center
            if label == 'A': lx -= 0.35; ly += 0.2
            elif label == 'B': lx += 0.35; ly += 0.2
            elif label == 'C': ly -= 0.35
            ax.text(lx, ly, label, ha='center', va='center',
                    fontsize=14, fontweight='bold', color=OUTLINE_COLOR, zorder=4)

# --- МЕТОД 2: МАТЕМАТИЧНА СІТКА (Тільки для складного фіналу) ---
def render_logical_area_sketchy(ax, centers, radius, logic_func):
    res = 500
    x = np.linspace(-1.6, 1.6, res)
    y = np.linspace(-1.6, 1.6, res)
    X, Y = np.meshgrid(x, y)
    
    def in_circle(cx, cy): return (X - cx)**2 + (Y - cy)**2 <= radius**2

    mask_A = in_circle(*centers[0])
    mask_B = in_circle(*centers[1])
    mask_C = in_circle(*centers[2])
    
    Z = logic_func(mask_A, mask_B, mask_C).astype(float)
    
    # Малюємо штрихування через contourf
    cnt = ax.contourf(X, Y, Z, levels=[0.5, 1.5], hatches=[HATCH_PATTERN], colors='none', extend='neither')
    for collection in ax.collections:
        collection.set_edgecolor(HATCH_COLOR)
        collection.set_linewidth(0)

def visualize_hybrid(data):
    U, A_set, B_set, C_set = data['U'], data['A'], data['B'], data['C']
    
    diff_AB = A_set - B_set
    diff_BC = B_set - C_set
    result = diff_AB | diff_BC
    
    fig, axes = plt.subplots(2, 2, figsize=(14, 12), facecolor=BG_COLOR)
    fig.suptitle(f'Візуалізація: ${data["formula"]}$', fontsize=18, color=OUTLINE_COLOR)

    # Координати
    cA, cB, cC = (-0.5, 0.2), (0.5, 0.2), (0, -0.6)
    centers = [cA, cB, cC]
    radius = 0.7

    # --- Plot 1: Початкові множини (Стиль: Sketch/Outline) ---
    ax = axes[0, 0]
    setup_ax_style(ax, 'Універсум та множини')
    rect = patches.Rectangle((-1.5, -1.5), 3, 3, facecolor='none', edgecolor=OUTLINE_COLOR, linewidth=2)
    rect.set_sketch_params(*SKETCH_OPTS)
    ax.add_patch(rect)
    ax.text(-1.3, 1.3, 'U', fontsize=16)
    
    draw_sketch_circle(ax, cA, radius, 'A', 'outline')
    draw_sketch_circle(ax, cB, radius, 'B', 'outline')
    draw_sketch_circle(ax, cC, radius, 'C', 'outline')

    # --- Plot 2: A \ B (Стиль: Sketch/Layers - виглядає акуратніше) ---
    ax = axes[0, 1]
    setup_ax_style(ax, 'Крок 1: A \\ B')
    
    # 1. Штрихуємо A
    draw_sketch_circle(ax, cA, radius, layer='hatch')
    # 2. Маскуємо B (біла пляма)
    draw_sketch_circle(ax, cB, radius, layer='mask')
    # 3. Контури зверху
    draw_sketch_circle(ax, cA, radius, 'A', 'outline')
    draw_sketch_circle(ax, cB, radius, 'B', 'outline')
    
    ax.text(0, -1.3, f'Результат: {sorted(list(diff_AB))}', ha='center', 
            bbox=dict(boxstyle='round', facecolor='white', edgecolor=OUTLINE_COLOR))

    # --- Plot 3: B \ C (Стиль: Sketch/Layers) ---
    ax = axes[1, 0]
    setup_ax_style(ax, 'Крок 2: B \\ C')
    
    # 1. Штрихуємо B
    draw_sketch_circle(ax, cB, radius, layer='hatch')
    # 2. Маскуємо C
    draw_sketch_circle(ax, cC, radius, layer='mask')
    # 3. Контури
    draw_sketch_circle(ax, cB, radius, 'B', 'outline')
    draw_sketch_circle(ax, cC, radius, 'C', 'outline')
    
    ax.text(0, -1.3, f'Результат: {sorted(list(diff_BC))}', ha='center',
            bbox=dict(boxstyle='round', facecolor='white', edgecolor=OUTLINE_COLOR))

    # --- Plot 4: Фінал (Стиль: Math Grid - бо форма складна) ---
    ax = axes[1, 1]
    setup_ax_style(ax, "Фінал: Об'єднання (Union)")
    
    # 1. Математично вираховуємо складну зону і штрихуємо її
    def final_logic(a, b, c):
        part1 = np.logical_and(a, np.logical_not(b))
        part2 = np.logical_and(b, np.logical_not(c))
        return np.logical_or(part1, part2)
    
    render_logical_area_sketchy(ax, centers, radius, final_logic)
    
    # 2. Малюємо контури поверх штриховки
    draw_sketch_circle(ax, cA, radius, 'A', 'outline')
    draw_sketch_circle(ax, cB, radius, 'B', 'outline')
    draw_sketch_circle(ax, cC, radius, 'C', 'outline')
    
    ax.text(0, -1.3, f'Фінальний результат: {sorted(list(result))}', ha='center', fontsize=12, fontweight='bold',
            bbox=dict(boxstyle='round,pad=0.3', facecolor='white', edgecolor=OUTLINE_COLOR))

    plt.tight_layout()
    plt.subplots_adjust(bottom=0.1)
    return fig

if __name__ == "__main__":
    print("🚀 Генерація гібридної візуалізації...")
    
    mpl.rcParams['font.family'] = 'DejaVu Sans'
    mpl.rcParams['hatch.linewidth'] = 2.0 

    try:
        OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    except:
        OUTPUT_DIR = Path(".")

    data = get_hardcoded_data()
    plt.switch_backend('Agg')
    
    fig = visualize_hybrid(data)
    save_path = OUTPUT_DIR / 'hybrid_venn.png'
    fig.savefig(save_path, dpi=150, bbox_inches='tight', facecolor=BG_COLOR)
    
    print(f"✅ Готово! Збережено як hybrid_venn.png у {save_path}")