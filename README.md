<div dir="rtl">

# Utilities, Extensions, Helpers and most useful features in all projects (web/desktop) in .Net

## API

- کلاس ApiResponse و متدهای کمکی جهت استفاده در پاسخ api ها
- متدهای کمکی برای صفحه بندی، بازگشت انواع mvc result متناسب با کد
- Global Exception Handler جهت هندل کردن استثنا های مدیریت نشده و لاگ
- مدل های مختلف Request Parameters برای دریافت ورودی، صفجه بندی و فیلتر

## Contracts

### MessageContract
کلاس و متدهای کمکی جهت انتقال ریزالت های پویا درون سرویسی یا متد به متد و یا به ریسپانس
  - MessageContractBase
  - MessageContract / MessageContract<T>

### Domain Contracts
- موجودیت های پایه برای استفاده در انتیتی ها. شامل:
  - BaseEntity / BaseEntity<TKey>, IBaseEntity
  - AuditableBaseEntity /  AuditableBaseEntity<TKey>, IAuditableBaseEntity
  - SoftDeletableBaseEntity / ISoftDeletableBaseEntity

## Localized Resources

- ریسورس های چند زبانه پیام های خطا، اطلاع رسانی، اسامی فیلدها و ...

## String Extensions

- کلاس های کمکی برای کار با رشته ها، پردازش رشته ها، تولید کدهای تصادفی، لینک
- محاسبه هش، تبدیل واحد ها

## Files

- رمز نگاری، کار با تصاویر، ولیدیشن آپلود



Saeed Rezayi.