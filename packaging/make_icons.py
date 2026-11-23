"""Generate a compact AI-chip icon; no ring, legible at Dock size."""
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont

out = Path(__file__).resolve().parents[1] / 'OpenAiUsage' / 'Assets'
out.mkdir(parents=True, exist_ok=True)
for size, name in [(96,'usage.png'),(44,'Square44x44Logo.png'),(150,'Square150x150Logo.png'),(50,'StoreLogo.png')]:
    s = size * 4
    image = Image.new('RGBA', (s,s), (0,0,0,0))
    draw = ImageDraw.Draw(image)
    # Solid rounded chip, not a thin outlined circle that disappears at 20px.
    box = (int(s*.12), int(s*.12), int(s*.88), int(s*.88))
    draw.rounded_rectangle(box, radius=int(s*.18), fill=(16,151,143,255))
    font = ImageFont.truetype('C:/Windows/Fonts/segoeuib.ttf', int(s*.41))
    draw.text((s/2,s*.49), 'AI', font=font, anchor='mm', fill=(255,255,255,255))
    r = int(s*.047)
    draw.ellipse((int(s*.77)-r,int(s*.23)-r,int(s*.77)+r,int(s*.23)+r),fill=(243,209,87,255))
    image.resize((size,size),Image.Resampling.LANCZOS).save(out/name)
