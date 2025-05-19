INSERT INTO "Products" ("Id", "Title", "Price", "Description", "Category", "ImageUrl", "RatingRate", "RatingCount", "CreatedAt")
VALUES 
    ('5a20d735-94bb-4587-9ca0-965a9b56d789', 'Skol 350ml', 3.50, 'Cerveja Pilsen', 'Bebidas', 'https://example.com/skol.jpg', 4.2, 150, NOW()),
    ('3490b7f2-6800-48a8-a666-e2885db7851b', 'Brahma 600ml', 5.90, 'Cerveja Lager', 'Bebidas', 'https://example.com/brahma.jpg', 4.5, 200, NOW()),
    (gen_random_uuid(), 'Antarctica 1L', 8.50, 'Cerveja Pilsen', 'Bebidas', 'https://example.com/antarctica.jpg', 4.0, 180, NOW()),
    (gen_random_uuid(), 'Colorado Appia 600ml', 12.90, 'Cerveja Artesanal', 'Bebidas', 'https://example.com/colorado.jpg', 4.8, 90, NOW()),
    (gen_random_uuid(), 'Patagonia Amber Lager 740ml', 15.50, 'Cerveja Artesanal', 'Bebidas', 'https://example.com/patagonia.jpg', 4.7, 75, NOW())
ON CONFLICT DO NOTHING;